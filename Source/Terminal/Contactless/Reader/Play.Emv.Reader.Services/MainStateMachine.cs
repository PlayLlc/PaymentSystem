﻿using System;

using Play.Ber.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Services.States;
using Play.Emv.Selection.Contracts;
using Play.Messaging;

namespace Play.Emv.Reader.Services;

internal class MainStateMachine
{
    #region Instance Values

    private readonly IEndpointClient _EndpointClient;
    private readonly IProcessOutcome _OutcomeProcessor;
    private readonly MainSessionLock _Lock;

    #endregion

    #region Constructor

    public MainStateMachine(ReaderDatabase readerDatabase, IEndpointClient endpointClient)
    {
        _EndpointClient = endpointClient;
        _OutcomeProcessor = new OutcomeProcessor(endpointClient);
        _Lock = new MainSessionLock(readerDatabase);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="TerminalException"></exception>
    public void Handle(ActivateReaderRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.State is not AwaitingTransaction)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateReaderRequest)} can't be processed because the state of the {nameof(MainStateMachine)} is not in the {nameof(AwaitingTransaction)} state");
            }

            _Lock.State = new AwaitingSelection(request.Transaction.GetTransactionSessionId(), request.GetCorrelationId(), _Lock.State.ReaderDatabase);
            _Lock.State.ReaderDatabase.Activate(request.Transaction.GetTransactionSessionId());
            _Lock.State.ReaderDatabase.Update(request.Transaction.AsPrimitiveValues());
            _EndpointClient.Send(new ActivateSelectionRequest(request.Transaction));
        }
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalException"></exception>
    public void Handle(OutSelectionResponse response)
    {
        lock (_Lock)
        {
            if (_Lock.State is not AwaitingSelection state)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutSelectionResponse)} can't be processed because the transaction is not in the {nameof(AwaitingSelection)} state");
            }

            _EndpointClient.Send(new StopSelectionRequest(state.TransactionSessionId));

            if (response.GetErrorIndication().IsErrorPresent())
            {
                ProcessOutcome(state, response.GetTransaction());

                return;
            }

            _Lock.State = StartKernel(state, response);
        }
    }

    private void ProcessOutcome(AwaitingSelection state, Transaction transaction)
    {
        _OutcomeProcessor.Process(state.CorrelationId!, state.TransactionSessionId, state.ReaderDatabase.GetTransaction());
    }

    /// <exception cref="TerminalException"></exception>
    private AwaitingKernel StartKernel(AwaitingSelection state, OutSelectionResponse selectionResponse)
    {
        if (selectionResponse.GetCombinationCompositeKey() is null)
        {
            throw new TerminalException(
                $"The {nameof(MainStateMachine)} attempted to start a kernel without a {nameof(CombinationCompositeKey)} in the {nameof(OutSelectionResponse)}");
        }

        if (selectionResponse.GetApplicationFileInformationResponse() is null)
        {
            throw new TerminalException(
                $"The {nameof(MainStateMachine)} attempted to start a kernel without a {nameof(CombinationCompositeKey)} in the {nameof(OutSelectionResponse)}");
        }

        KernelSessionId kernelSessionId = new(selectionResponse.GetKernelId()!, state.TransactionSessionId);
        ActivateKernelRequest activateKernelRequest = new(kernelSessionId,
            state.ReaderDatabase.GetKernelValues(selectionResponse.GetCombinationCompositeKey()!), selectionResponse.GetApplicationFileInformationResponse()!);

        _EndpointClient.Send(activateKernelRequest);

        return new AwaitingKernel(kernelSessionId, state.TransactionSessionId, state.CorrelationId, state.ReaderDatabase);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Handle(OutKernelResponse request)
    {
        lock (_Lock)
        {
            if (_Lock.State is not AwaitingKernel state)
                throw new RequestOutOfSyncException($"The {nameof(OutSelectionResponse)} can't be processed because the transaction is no longer processing");

            _EndpointClient.Send(new StopKernelRequest(state.KernelSessionId));
            _OutcomeProcessor.Process(state.CorrelationId!, request.GetKernelSessionId().GetTransactionSessionId(), request.GetTransaction());
        }
    }

    public void Handle(StopPcdAcknowledgedResponse response)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(StopReaderRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.State is AwaitingTransaction)
                throw new RequestOutOfSyncException($"The {nameof(OutSelectionResponse)} can't be processed because the transaction is no longer processing");

            // HACK: Send STOP signal if Selection process is active
            //if(Selection Endpoint is Active)
            //_SelectionEndpoint.Request(new ActivateSelectionRequest(_MainSessionLock.Session.Transaction)); 

            // HACK: Send STOP signal if PCD is active
            //if(PCD is Active)
            //_PCD.Request(new StopPcdRequest(_MainSessionLock.Session.Transaction));

            if (_Lock.State is AwaitingKernel kernelState)
                _EndpointClient.Send(new StopKernelRequest(kernelState.KernelSessionId));
            else if (_Lock.State is AwaitingSelection selectionState)
                _EndpointClient.Send(new StopSelectionRequest(selectionState.TransactionSessionId));
            else
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(StopReaderRequest)} can't be processed because the {nameof(MainStateMachine)} is in an unknown state");
            }

            _Lock.State = new AwaitingTransaction(_Lock.State.ReaderDatabase);
        }
    }

    #endregion

    public class MainSessionLock
    {
        #region Instance Values

        public MainState State;

        #endregion

        #region Constructor

        public MainSessionLock(ReaderDatabase database)
        {
            State = new AwaitingTransaction(database);
        }

        #endregion
    }
}