using System;

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

    private readonly TerminalSessionLock _TerminalSessionLock;
    private readonly TerminalConfiguration _TerminalConfiguration;
    private readonly IHandleDisplayRequests _DisplayEndpoint;
    private readonly IHandleKernelRequests _KernelEndpoint;
    private readonly IHandleReaderRequests _ReaderEndpoint;
    private readonly IPerformTerminalActionAnalysis _TerminalActionAnalysisService;
    private readonly IManageTerminalRisk _TerminalRiskManager;
    private readonly ITerminalConfigurationRepository _TerminalConfigurationRepository;
    private readonly ISendTerminalResponses _TerminalEndpoint;
    private readonly IGenerateSequenceTraceAuditNumbers _SequenceGenerator;

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
        IGenerateSequenceTraceAuditNumbers sequenceGenerator)
    {
        _TerminalSessionLock = new TerminalSessionLock(sequenceGenerator);
        _TerminalConfiguration = terminalConfiguration;
        _DisplayEndpoint = displayEndpoint;
        _KernelEndpoint = kernelEndpoint;
        _ReaderEndpoint = readerEndpoint;
        _TerminalActionAnalysisService = terminalActionAnalysisService;
        _TerminalRiskManager = terminalRiskManager;
        _TerminalConfigurationRepository = terminalConfigurationRepository;
        _TerminalEndpoint = terminalEndpoint;
        _SequenceGenerator = sequenceGenerator;
    }

    #endregion

    #region Instance Members

    // TODO: I'm not sure if this is the correct way to implement this. I'm not sure what form of communication the point of sale will have with the Terminal. Keeping this here for now
    public void Handle(ActivateTerminalRequest request)
    {
        lock (_TerminalSessionLock)
        {
            if (_TerminalSessionLock.Session != null)
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

            _TerminalSessionLock.Session = new TerminalSession(transaction, _TerminalConfiguration,
                new DataExchangeTerminalService(transaction.GetTransactionSessionId(), _TerminalEndpoint, _KernelEndpoint));

            // HACK: Develop logic for passing TagsToRead and DataToSend along with the ACT signal below

            var dataToSend = new DataToSend(new PosEntryMode(PosEntryModeTypes.EmvModes.Contactless));

            _ReaderEndpoint.Request(new ActivateReaderRequest(transaction,));
        }
    }

    public void Handle(QueryTerminalRequest request)
    {
        lock (_TerminalSessionLock)
        {
            if (_TerminalSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} doesn't currently have an active session");
            }

            if (_TerminalSessionLock.Session.GetTransactionSessionId() != request.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.GetTransactionSessionId()}] but the current {nameof(ChannelType.Terminal)} session has a {nameof(TransactionSessionId)} of: [{_TerminalSessionLock.Session.GetTransactionSessionId()}]");
            }

            _TerminalSessionLock.Session.DataExchangeTerminalService.Enqueue(request.GetDataNeeded());
            _TerminalSessionLock.Session.DataExchangeTerminalService.Resolve(in _TerminalSessionLock.Session);
        }
    }

    public void Handle(OutReaderResponse response)
    {
        throw new NotImplementedException();
    }

    public void Handle(QueryKernelResponse response)
    {
        throw new NotImplementedException();
    }

    public void Handle(StopReaderAcknowledgedResponse response)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private Helper Methods

    #endregion

    public class TerminalSessionLock
    {
        #region Instance Values

        public TerminalSession? Session;

        // BUG: Generate STAN for each transaction in a settlement batch. The sequence will be restarted when a Settlement request has been successfully acknowledged by the Acquirer
        public IGenerateSequenceTraceAuditNumbers SequenceGenerator;

        #endregion

        #region Constructor

        public TerminalSessionLock(IGenerateSequenceTraceAuditNumbers sequenceGenerator)
        {
            SequenceGenerator = sequenceGenerator;
        }

        #endregion
    }
}