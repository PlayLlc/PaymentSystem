using System;
using System.Collections.Generic;

using Play.Emv.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Messaging;
using Play.Emv.Reader.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Common.Services.SequenceNumberManagement;
using Play.Emv.Terminal.Configuration;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Services.DataExchange;
using Play.Emv.Transactions;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Services;

internal class TerminalStateMachine
{
    #region Instance Values

    private readonly TerminalSessionLock _Lock;
    private readonly TerminalConfiguration _TerminalConfiguration;
    private readonly DataExchangeTerminalService _DataExchangeTerminalService;
    private readonly IHandleDisplayRequests _DisplayEndpoint;
    private readonly IHandleKernelRequests _KernelEndpoint;
    private readonly IHandleReaderRequests _ReaderEndpoint;
    private readonly IPerformTerminalActionAnalysis _TerminalActionAnalysisService;
    private readonly IManageTerminalRisk _TerminalRiskManager;
    private readonly ITerminalConfigurationRepository _TerminalConfigurationRepository;
    private readonly ISendTerminalResponses _TerminalEndpoint;
    private readonly IGenerateSequenceTraceAuditNumbers _SequenceGenerator;
    private readonly IHandleAcquirerRequests _AcquirerEndpoint;

    #endregion

    #region Constructor

    public TerminalStateMachine(
        TerminalConfiguration terminalConfiguration,
        IHandleDisplayRequests displayEndpoint,
        IHandleKernelRequests kernelEndpoint,
        IHandleReaderRequests readerEndpoint,
        IPerformTerminalActionAnalysis terminalActionAnalysisService,
        IManageTerminalRisk terminalRiskManager,
        ITerminalConfigurationRepository terminalConfigurationRepository,
        ISendTerminalResponses terminalEndpoint,
        IGenerateSequenceTraceAuditNumbers sequenceGenerator,
        IHandleAcquirerRequests acquirerEndpoint,
        DataExchangeTerminalService dataExchangeTerminalService)
    {
        _Lock = new TerminalSessionLock();
        _TerminalConfiguration = terminalConfiguration;
        _DisplayEndpoint = displayEndpoint;
        _KernelEndpoint = kernelEndpoint;
        _ReaderEndpoint = readerEndpoint;
        _TerminalActionAnalysisService = terminalActionAnalysisService;
        _TerminalRiskManager = terminalRiskManager;
        _TerminalConfigurationRepository = terminalConfigurationRepository;
        _TerminalEndpoint = terminalEndpoint;
        _SequenceGenerator = sequenceGenerator;
        _AcquirerEndpoint = acquirerEndpoint;
        _DataExchangeTerminalService = dataExchangeTerminalService;
    }

    #endregion

    #region Instance Members

    // TODO: I'm not sure if this is the correct way to implement this. I'm not sure what form of communication the point of sale will have with the Terminal. Keeping this here for now
    public void Handle(ActivateTerminalRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.IsActive())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} already has an active session in progress");
            }

            // HACK: This won't change unless it's a cloud kernel implementation. Find a better strategy
            TerminalConfiguration systemConfiguration =
                _TerminalConfigurationRepository.GeTerminalConfiguration(request.GetTerminalIdentification(),
                    request.GetAcquirerIdentifier(), request.GetMerchantIdentifier());

            Transaction transaction = new(new TransactionSessionId(request.GetTransactionType()), request.GetAccountType(),
                request.GetAmountAuthorizedNumeric(), request.GetAmountOtherNumeric(), request.GetTransactionType(),
                systemConfiguration.GetLanguagePreference(), systemConfiguration.GetTerminalCountryCode(),
                new TransactionDate(DateTimeUtc.Now()), new TransactionTime(DateTimeUtc.Now()));

            _Lock.Session = new TerminalSession(_SequenceGenerator.Generate(), request.GetMessageTypeIndicator(), transaction);

            // HACK: Develop logic for passing TagsToRead and DataToSend along with the ACT signal below

            DataToSend? dataToSend = new(request.GetPosEntryMode());

            _ReaderEndpoint.Request(new ActivateReaderRequest(transaction, dataToSend));
        }
    }

    public void Handle(AcquirerResponseSignal request)
    {
        // BUG: Implement this callback handler
        throw new NotImplementedException();
    }

    // HACK: This should be handled on a separate process
    public void Handle(QueryTerminalRequest request)
    {
        lock (_Lock)
        {
            if (!_Lock.IsActive())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} doesn't currently have an active session");
            }

            if (_Lock.Session!.GetTransactionSessionId() != request.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.GetTransactionSessionId()}] but the current {nameof(ChannelType.Terminal)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _DataExchangeTerminalService.Enqueue(request.GetDataNeeded());
            _DataExchangeTerminalService.Resolve(in _Lock.Session);
        }
    }

    public void Handle(OutReaderResponse response)
    {
        lock (_Lock)
        {
            if (!_Lock.IsActive())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} doesn't currently have an active session");
            }

            if (_Lock.Session!.GetTransactionSessionId() != response.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(ChannelType.Terminal)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _Lock.Session.SetFinalOutcome(response.GetOutcome());

            IssuerMessageFactory factory = _AcquirerEndpoint.GetMessageFactory(_Lock.Session!.MessageTypeIndicator);
            DataNeeded neededData = factory.GetDataNeeded(_Lock.Session!.MessageTypeIndicator);

            _DataExchangeTerminalService.Enqueue(neededData);
            _DataExchangeTerminalService.QueryKernel(_Lock.Session.GetTransactionSessionId(), response.GetKernelSessionId().GetKernelId());
        }

        throw new NotImplementedException();
    }

    public void Handle(QueryKernelResponse response)
    {
        lock (_Lock)
        {
            if (_Lock.IsAwaitingAcquirerResponse())
            {
                _AcquirerEndpoint.Request(new AcquirerRequestSignal(_Lock.Session!.MessageTypeIndicator,
                    response.GetDataToSend().AsTagLengthValueArray()));
            }
        }

        throw new NotImplementedException();
    }

    public void Handle(StopReaderAcknowledgedResponse response)
    {
        throw new NotImplementedException();
    }

    #endregion

    public class TerminalSessionLock
    {
        #region Instance Values

        public TerminalSession? Session;

        #endregion

        #region Constructor

        public TerminalSessionLock()
        {
            Session = null;
        }

        #endregion

        #region Instance Members

        public bool IsActive() => GetState() != TerminalState.Idle;
        public bool IsAwaitingAcquirerResponse() => GetState() == TerminalState.AwaitingAcquirerResponse;

        private TerminalState GetState()
        {
            if (Session == null)
                return TerminalState.Idle;

            if (Session.FinalOutcome != null)
                return TerminalState.AwaitingAcquirerResponse;

            return TerminalState.Processing;
        }

        #endregion

        public enum TerminalState
        {
            Idle,
            Processing,
            AwaitingAcquirerResponse
        }

        // BUG: Generate STAN for each transaction in a settlement batch. The sequence will be restarted when a Settlement request has been successfully acknowledged by the Acquirer
    }
}