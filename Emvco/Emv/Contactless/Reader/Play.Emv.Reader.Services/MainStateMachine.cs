using System;

using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Contracts.SignalIn;
using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Kernel.Services;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Selection.Contracts;
using Play.Emv.Selection.Contracts.SignalIn;
using Play.Emv.Selection.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Transactions;
using Play.Messaging;

namespace Play.Emv.Reader.Services;

internal class MainStateMachine
{
    #region Instance Values

    private readonly IHandleSelectionRequests _SelectionEndpoint;
    private readonly IReaderEndpoint _ReaderEndpoint;
    private readonly IProcessOutcome _OutcomeProcessor;
    private readonly KernelRetriever _KernelRetriever;
    private readonly MainSessionLock _MainSessionLock = new();

    #endregion

    #region Constructor

    public MainStateMachine(
        IHandleSelectionRequests selectionEndpoint,
        KernelRetriever kernelRetriever,
        IHandleDisplayRequests displayEndpoint,
        IReaderEndpoint readerEndpoint)
    {
        _ReaderEndpoint = readerEndpoint;
        _SelectionEndpoint = selectionEndpoint;
        _KernelRetriever = kernelRetriever;
        _OutcomeProcessor = new OutcomeProcessor(selectionEndpoint, displayEndpoint, readerEndpoint);
    }

    #endregion

    #region Instance Members

    public void Handle(ActivateReaderRequest request)
    {
        lock (_MainSessionLock)
        {
            if (_MainSessionLock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateReaderRequest)} can't be processed because the {nameof(TransactionSessionId)}: [{_MainSessionLock.Session!.GetTransactionSessionId()}] is currently processing");
            }

            _MainSessionLock.Session = new MainSession(request.GetCorrelationId(), request.GetTransaction(), request.GetTagsToRead(), null);
            _SelectionEndpoint.Request(new ActivateSelectionRequest(request.GetTransaction()));
        }
    }

    public void Handle(OutSelectionResponse request)
    {
        lock (_MainSessionLock)
        {
            if (_MainSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutSelectionResponse)} can't be processed because the transaction is no longer processing");
            }

            if (_MainSessionLock.Session.KernelSessionId != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutSelectionResponse)} can't be processed because an active Kernel still exists for this session");
            }

            KernelSessionId kernelSessionId = new(request.GetKernelId(), _MainSessionLock.Session.GetTransactionSessionId());

            _MainSessionLock.Session = _MainSessionLock.Session with {KernelSessionId = kernelSessionId};

            ActivateKernelRequest activateKernelRequest = new(kernelSessionId, request.GetCombinationCompositeKey(),
                request.GetTransaction(), _MainSessionLock?.Session.TagsToRead, request.GetTerminalTransactionQualifiers(),
                request.GetApplicationFileInformationResponse(), request.GetApplicationFileInformationResponse().GetStatusWords());

            _KernelRetriever.Enqueue(activateKernelRequest);
        }
    }

    public void Handle(OutKernelResponse request)
    {
        lock (_MainSessionLock)
        {
            if (_MainSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutKernelResponse)} can't be processed because the transaction is no longer processing");
            }

            if (!_MainSessionLock.TryGetKernelSessionId(out KernelSessionId? kernelSessionId))
            {
                // The lifetime of the Kernel is managed here Process M. The active kernel should not have stopped at this point
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutKernelResponse)} can't be processed because the Kernel is no longer active");
            }

            _OutcomeProcessor.Process(_MainSessionLock.Session.ActSignalCorrelationId, _MainSessionLock.Session.Transaction);
            _KernelRetriever.Enqueue(new StopKernelRequest(kernelSessionId!.Value));
        }
    }

    public void Handle(StopPcdAcknowledgedResponse response)
    {
        throw new NotImplementedException();
    }

    public void Handle(StopReaderRequest request)
    {
        lock (_MainSessionLock)
        {
            if (_MainSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutKernelResponse)} can't be processed because the transaction is no longer processing");
            }

            // HACK: Send STOP signal if Selection process is active
            //if(Selection Endpoint is Active)
            //_SelectionEndpoint.Request(new ActivateSelectionRequest(_MainSessionLock.Session.Transaction)); 

            // HACK: Send STOP signal if PCD is active
            //if(PCD is Active)
            //_PCD.Request(new StopPcdRequest(_MainSessionLock.Session.Transaction));

            if (_MainSessionLock.TryGetKernelSessionId(out KernelSessionId? kernelSessionId))
                _KernelRetriever.Enqueue(new StopKernelRequest(kernelSessionId!.Value));

            _OutcomeProcessor.Process(_MainSessionLock.Session.ActSignalCorrelationId, _MainSessionLock.Session.Transaction);
        }
    }

    #endregion

    public class MainSessionLock
    {
        #region Instance Values

        public MainSession? Session;

        #endregion

        #region Instance Members

        public bool TryGetKernelSessionId(out KernelSessionId? result)
        {
            if (Session?.KernelSessionId != null)
            {
                result = Session!.KernelSessionId;

                return true;
            }

            result = null;

            return false;
        }

        public bool TryGetTransaction(out Transaction? result)
        {
            if (Session?.Transaction != null)
            {
                result = Session!.Transaction;

                return true;
            }

            result = null;

            return false;
        }

        public bool TryGetTagsToRead(out TagsToRead? result)
        {
            if (Session?.TagsToRead != null)
            {
                result = Session!.TagsToRead;

                return true;
            }

            result = null;

            return false;
        }

        public bool TryGetActSignalCorrelationId(out CorrelationId? result)
        {
            if (Session?.ActSignalCorrelationId != null)
            {
                result = Session!.ActSignalCorrelationId;

                return true;
            }

            result = null;

            return false;
        }

        #endregion
    }
}