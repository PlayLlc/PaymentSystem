﻿using Play.Acquirer.Contracts;
using Play.Ber.DataObjects;
using Play.Emv.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Reader.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Common.Services.SequenceNumberManagement;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Services.DataExchange;
using Play.Emv.Transactions;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.StateMachine;

public interface ISettlementReconciliationService
{
    public AcquirerRequestSignal CreateSettlementRequest(AcquirerMessageFactory messageFactory, DateTimeUtc settlementRequestTimeUtc);
}

/// <summary>
///     This state handles Terminal requests when a transaction session is not currently being processed
/// </summary>
internal class Idle : TerminalState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(Idle));

    #endregion

    #region Instance Values

    private readonly IHandleReaderRequests _ReaderEndpoint;
    private readonly ISettlementReconciliationService _SettlementReconciliationService;
    private readonly IGetTerminalState _TerminalStateResolver;
    private readonly IGenerateSequenceTraceAuditNumbers _SequenceGenerator;
    private readonly IHandleAcquirerRequests _AcquirerEndpoint;
    private readonly TerminalConfiguration _TerminalConfiguration;

    #endregion

    #region Constructor

    public Idle(
        DataExchangeTerminalService dataExchangeTerminalService,
        TerminalConfiguration terminalConfiguration,
        IGetTerminalState terminalStateResolver,
        ISettlementReconciliationService settlementReconciliationService,
        IGenerateSequenceTraceAuditNumbers sequenceGenerator,
        IHandleAcquirerRequests acquirerEndpoint) : base(dataExchangeTerminalService)
    {
        _TerminalConfiguration = terminalConfiguration;
        _TerminalStateResolver = terminalStateResolver;
        _SettlementReconciliationService = settlementReconciliationService;
        _SequenceGenerator = sequenceGenerator;
        _AcquirerEndpoint = acquirerEndpoint;
    }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    public TerminalState Handle(InitiateSettlementRequest signal)
    {
        AcquirerMessageFactory settlementRequestFactory =
            _AcquirerEndpoint.GetMessageFactory(MessageTypeIndicatorTypes.Reconciliation.ReconciliationRequest);

        AcquirerRequestSignal settlementRequest =
            _SettlementReconciliationService.CreateSettlementRequest(settlementRequestFactory, signal.SettlementRequestDateTimeUtc);

        _AcquirerEndpoint.Request(settlementRequest);

        return _TerminalStateResolver.GetKernelState(WaitingForSettlementResponse.StateId);
    }

    public override TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal)
    {
        // HACK: Develop logic for passing TagsToRead and DataToSend along with the ACT signal below

        DataToSend? dataToSend = new(signal.GetPosEntryMode());

        _ReaderEndpoint.Request(new ActivateReaderRequest(session.Transaction, dataToSend));

        return _TerminalStateResolver.GetKernelState(WaitingForFinalOutcome.StateId);
    }

    public override TerminalState Handle(TerminalSession session, OutReaderResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    public override TerminalState Handle(TerminalSession session, QueryKernelResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    public override TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    /// <summary>
    ///     There shouldn't be any incoming responses from the Acquirer while we're in the <see cref="Idle" /> state. There are
    ///     some instances where the Acquirer can send requests directly to the terminal for operations such as file
    ///     management, but we should not expect a reply from the terminal in this state
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, AcquirerResponseSignal signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    public override TerminalState Handle(TerminalSession session, QueryTerminalRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    #endregion
}