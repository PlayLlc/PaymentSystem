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
using Play.Emv.Selection.Contracts;
using Play.Messaging;

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
            if (_Lock.ReaderDatabase.TryGetTransactionSessionId(out TransactionSessionId? sessionId))
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateReaderRequest)} can't be processed because the {nameof(TransactionSessionId)}: [{sessionId}] is currently processing");
            }

            _Lock.ReaderDatabase.Activate(request.GetTransaction().GetTransactionSessionId());
            _Lock.ReaderDatabase.Update(request.GetTransaction().AsPrimitiveValues());
            _Lock.CorrelationId = request.GetCorrelationId();

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
            if (!_Lock.ReaderDatabase.TryGetTransactionSessionId(out TransactionSessionId? transactionSessionId))
                throw new RequestOutOfSyncException($"The {nameof(OutSelectionResponse)} can't be processed because the transaction is no longer processing");

            if (_Lock.KernelSessionId != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutSelectionResponse)} can't be processed because an active Kernel still exists for this session");
            }

            if (request.GetErrorIndication().IsErrorPresent())
            {
                _OutcomeProcessor.Process(_Lock.CorrelationId!, _Lock.KernelSessionId!.Value, _Lock.ReaderDatabase.GetTransaction());
                _SelectionEndpoint.Request(new StopSelectionRequest(transactionSessionId!.Value));

                return;
            }

            KernelSessionId kernelSessionId = new(request.GetKernelId()!, transactionSessionId!.Value);

            _Lock.KernelSessionId = kernelSessionId;

            ActivateKernelRequest activateKernelRequest = new(kernelSessionId, request.GetCombinationCompositeKey()!, request.GetTransaction(),
                _Lock.ReaderDatabase.Get<TagsToRead>(TagsToRead.Tag), request.GetTerminalTransactionQualifiers()!,
                request.GetApplicationFileInformationResponse()!);

            _KernelRetriever.Enqueue(activateKernelRequest);
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="Ber.Exceptions.TerminalException"></exception>
    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    public void Handle(OutKernelResponse request)
    {
        lock (_Lock)
        {
            if (!_Lock.ReaderDatabase.IsActive())
                throw new RequestOutOfSyncException($"The {nameof(OutSelectionResponse)} can't be processed because the transaction is no longer processing");

            if (_Lock.KernelSessionId != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(OutSelectionResponse)} can't be processed because an active Kernel still exists for this session");
            }

            _OutcomeProcessor.Process(_Lock.CorrelationId!, request.GetKernelSessionId(), _Lock.ReaderDatabase.GetTransaction());
            _KernelRetriever.Enqueue(new StopKernelRequest(_Lock.KernelSessionId!.Value));
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
            if (!_Lock.ReaderDatabase.TryGetTransactionSessionId(out TransactionSessionId? transactionSessionId))
                throw new RequestOutOfSyncException($"The {nameof(OutSelectionResponse)} can't be processed because the transaction is no longer processing");

            // HACK: Send STOP signal if Selection process is active
            //if(Selection Endpoint is Active)
            //_SelectionEndpoint.Request(new ActivateSelectionRequest(_MainSessionLock.Session.Transaction)); 

            // HACK: Send STOP signal if PCD is active
            //if(PCD is Active)
            //_PCD.Request(new StopPcdRequest(_MainSessionLock.Session.Transaction));

            if (_Lock.KernelSessionId is not null)
                _KernelRetriever.Enqueue(new StopKernelRequest(_Lock.KernelSessionId!.Value));

            _OutcomeProcessor.Process(_Lock.CorrelationId!, _Lock.KernelSessionId!.Value, _Lock.ReaderDatabase.GetTransaction());
        }
    }

    #endregion

    public class MainSessionLock
    {
        #region Instance Values

        public readonly ReaderDatabase ReaderDatabase;

        // HACK - Need to save KernelSessionId to the Reader Database when initializing the Kernel. Save CorrelationId to the Reader when Activate()
        public KernelSessionId? KernelSessionId;
        public CorrelationId? CorrelationId;

        #endregion

        #region Constructor

        public MainSessionLock(ReaderDatabase database)
        {
            ReaderDatabase = database;
        }

        #endregion
    }
}