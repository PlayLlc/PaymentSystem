using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Services;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Database;
using Play.Emv.Reader.Services.States;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Reader.Services;

internal class MainStateMachine
{
    #region Instance Values

    private readonly IHandleSelectionRequests _SelectionEndpoint;
    private readonly IReaderEndpoint _ReaderEndpoint;
    private readonly IProcessOutcome _OutcomeProcessor;
    private readonly KernelRetriever _KernelRetriever;
    private readonly MainSessionLock _Lock;

    #endregion

    #region Constructor

    public MainStateMachine(
        ReaderDatabase readerDatabase, IHandleSelectionRequests selectionEndpoint, KernelRetriever kernelRetriever, IHandleDisplayRequests displayEndpoint,
        IReaderEndpoint readerEndpoint)
    {
        _ReaderEndpoint = readerEndpoint;
        _SelectionEndpoint = selectionEndpoint;
        _KernelRetriever = kernelRetriever;
        _OutcomeProcessor = new OutcomeProcessor(selectionEndpoint, displayEndpoint, readerEndpoint);
        _Lock = new MainSessionLock(readerDatabase);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(ActivateReaderRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.State is not AwaitingTransaction state)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateReaderRequest)} can't be processed because the state of the {nameof(MainStateMachine)} is not in the {nameof(AwaitingTransaction)} state");
            }

            state = new AwaitingSelection(request.GetTransaction().GetTransactionSessionId(), request.GetCorrelationId(), _Lock.State.ReaderDatabase);
            state.ReaderDatabase.Activate(request.GetTransaction().GetTransactionSessionId());
            state.ReaderDatabase.Update(request.GetTransaction().AsPrimitiveValues());
            _SelectionEndpoint.Request(new ActivateSelectionRequest(request.GetTransaction()));
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(OutSelectionResponse request)
    {
        lock (_Lock)
        {
            if (_Lock.State is not AwaitingSelection state)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutSelectionResponse)} can't be processed because the transaction is not in the {nameof(AwaitingSelection)} state");
            }

            _SelectionEndpoint.Request(new StopSelectionRequest(state.TransactionSessionId));

            if (request.GetErrorIndication().IsErrorPresent())
            {
                _OutcomeProcessor.Process(state.CorrelationId!, state.TransactionSessionId, state.ReaderDatabase.GetTransaction());

                return;
            }

            KernelSessionId kernelSessionId = new(request.GetKernelId()!, state.TransactionSessionId);

            _Lock.State = new AwaitingKernel(kernelSessionId, state.TransactionSessionId, state.CorrelationId, state.ReaderDatabase);

            ActivateKernelRequest activateKernelRequest = new(kernelSessionId, _Lock.State.ReaderDatabase.GetKernelValues(request.GetCombinationCompositeKey()),
                request.GetApplicationFileInformationResponse()!);

            // Entry Point D - Start Kernel
            _KernelRetriever.Enqueue(activateKernelRequest);
        }
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="Ber.Exceptions.TerminalException"></exception>
    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    public void Handle(OutKernelResponse request)
    {
        lock (_Lock)
        {
            if (_Lock.State is not AwaitingKernel state)
                throw new RequestOutOfSyncException($"The {nameof(OutSelectionResponse)} can't be processed because the transaction is no longer processing");

            _KernelRetriever.Enqueue(new StopKernelRequest(state.KernelSessionId));
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
                _KernelRetriever.Enqueue(new StopKernelRequest(kernelState.KernelSessionId));
            else if (_Lock.State is AwaitingSelection selectionState)
                _SelectionEndpoint.Request(new StopSelectionRequest(selectionState.TransactionSessionId));
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