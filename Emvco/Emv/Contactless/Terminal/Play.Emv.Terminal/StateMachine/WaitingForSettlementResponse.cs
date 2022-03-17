using System;

using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Messaging;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.DataExchange;
using Play.Emv.Terminal.Session;

namespace Play.Emv.Terminal.StateMachine;

public class WaitingForSettlementResponse : TerminalState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForAuthorizationResponse));

    #endregion

    #region Instance Values

    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly ISettlementReconciliationService _SettlementReconciliationService;
    private readonly IGetTerminalState _TerminalStateResolver;
    private readonly IGenerateSequenceTraceAuditNumbers _SequenceGenerator;

    #endregion

    #region Constructor

    public WaitingForSettlementResponse(
        DataExchangeTerminalService dataExchangeTerminalService,
        IHandleTerminalRequests terminalEndpoint,
        ISettlementReconciliationService settlementReconciliationService,
        IGetTerminalState terminalStateResolver,
        IGenerateSequenceTraceAuditNumbers sequenceGenerator) : base(dataExchangeTerminalService)
    {
        _TerminalEndpoint = terminalEndpoint;
        _SettlementReconciliationService = settlementReconciliationService;
        _TerminalStateResolver = terminalStateResolver;
        _SequenceGenerator = sequenceGenerator;
    }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;
    public override TerminalState Handle(TerminalSession? session, InitiateSettlementRequest signal) => throw new NotImplementedException();

    public override TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal)
    {
        _TerminalEndpoint.Request(signal);

        // BUG: There is not yet a timeout cancellation management set up yet in the terminal, so if the Acquirer never sends a response this will keep getting called indefinitely
        return _TerminalStateResolver.GetKernelState(Idle.StateId);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, OutReaderResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, QueryKernelResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession? session, AcquirerResponseSignal signal)
    {
        if (session != null)
        {
            throw new
                RequestOutOfSyncException($"The {nameof(ActivateTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} already has an active session in progress");
        }

        _SequenceGenerator.Reset(signal);

        return _TerminalStateResolver.GetKernelState(Idle.StateId);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, QueryTerminalRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Terminal);

    #endregion
}