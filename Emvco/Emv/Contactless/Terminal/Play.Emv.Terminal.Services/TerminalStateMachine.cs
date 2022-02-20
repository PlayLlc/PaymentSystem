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
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Messaging;
using Play.Emv.Reader.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Common.Services.SequenceNumberManagement;
using Play.Emv.Terminal.Configuration;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Services.DataExchange;
using Play.Emv.Terminal.StateMachine;
using Play.Emv.Transactions;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Services;

internal class TerminalStateMachine
{
    #region Instance Values

    private readonly TerminalStateLock _Lock;
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
    private readonly IGetTerminalState _TerminalStateResolver;

    #endregion

    #region Constructor

    public TerminalStateMachine(
        IGetTerminalState terminalStateResolver,
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
        _TerminalStateResolver = terminalStateResolver;
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

        _Lock = new TerminalStateLock(terminalStateResolver.GetKernelState(Idle.StateId));
    }

    #endregion

    #region Instance Members

    // TODO: I'm not sure if this is the correct way to implement this. I'm not sure what form of communication the point of sale will have with the Terminal. Keeping this here for now
    public void Handle(ActivateTerminalRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} already has an active session in progress");
            }

            Transaction transaction = new(new TransactionSessionId(request.GetTransactionType()), request.GetAccountType(),
                request.GetAmountAuthorizedNumeric(), request.GetAmountOtherNumeric(), request.GetTransactionType(),
                _TerminalConfiguration.GetLanguagePreference(), _TerminalConfiguration.GetTerminalCountryCode(),
                new TransactionDate(DateTimeUtc.Now()), new TransactionTime(DateTimeUtc.Now()));

            _Lock.Session = new TerminalSession(_SequenceGenerator.Generate(), request.GetMessageTypeIndicator(), transaction);
            _Lock.State = _Lock.State.Handle(_Lock.Session, request);
        }
    }

    public void Handle(AcquirerResponseSignal request)
    {
        lock (_Lock)
        {
            _Lock.State = _Lock.State.Handle(_Lock.Session, request);
        }
    }

    public void Handle(OutReaderResponse response)
    {
        lock (_Lock)
        {
            _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
        }
    }

    public void Handle(QueryKernelResponse response)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryKernelResponse)} can't be processed because the {nameof(TerminalStateMachine)} has an active session");
            }

            lock (_Lock)
            {
                _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
            }
        }
    }

    public void Handle(StopReaderAcknowledgedResponse response)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryKernelResponse)} can't be processed because the {nameof(TerminalStateMachine)} has an active session");
            }

            lock (_Lock)
            {
                _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
            }
        }
    }

    public void Handle(InitiateSettlementRequest response)
    {
        // call state 

        throw new NotImplementedException();
    }

    // HACK: This should be handled on a separate process
    public void Handle(QueryTerminalRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryKernelResponse)} can't be processed because the {nameof(TerminalStateMachine)} has an active session");
            }

            lock (_Lock)
            {
                _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
            }
        }
    }

    #endregion

    internal class TerminalStateLock
    {
        #region Instance Values

        public TerminalSession? Session;
        public TerminalState State;

        #endregion

        #region Constructor

        public TerminalStateLock(TerminalState state)
        {
            State = state;
        }

        #endregion
    }
}