using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Configuration;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.DataExchange;
using Play.Emv.Terminal.Session;
using Play.Messaging;

namespace Play.Emv.Terminal.StateMachine;

public class WaitingForSettlementResponse : TerminalState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForAuthorizationResponse));

    #endregion

    #region Instance Values

    private readonly IGenerateSequenceTraceAuditNumbers _SequenceGenerator;

    #endregion

    #region Constructor

    public WaitingForSettlementResponse(
        TerminalConfiguration terminalConfiguration, DataExchangeTerminalService dataExchangeTerminalService, IEndpointClient endpointClient,
        IGetTerminalState terminalStateResolver, IGenerateSequenceTraceAuditNumbers sequenceGenerator) : base(dataExchangeTerminalService,
        terminalConfiguration, endpointClient, terminalStateResolver)
    {
        _SequenceGenerator = sequenceGenerator;
    }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    public override TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal)
    {
        _EndpointClient.Send(signal);

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
    public override TerminalState Handle(TerminalSession session, OutReaderResponse signal) => throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, QueryKernelResponse signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession? session, ReconciliationResponse signal)
    {
        if (session != null)
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(ActivateTerminalRequest)} can't be processed because the {nameof(TerminalChannel.Id)} already has an active session in progress");
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
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    #endregion
}