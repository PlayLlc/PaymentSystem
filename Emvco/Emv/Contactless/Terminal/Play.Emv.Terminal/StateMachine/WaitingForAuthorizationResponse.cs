using System;

using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.DataExchange;
using Play.Emv.Terminal.Session;

namespace Play.Emv.Terminal.StateMachine;

/// <summary>
///     This state transition handles Terminal requests when it is in the process of communicating with the Acquirer
/// </summary>
public class WaitingForAuthorizationResponse : TerminalState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForAuthorizationResponse));

    #endregion

    #region Constructor

    public WaitingForAuthorizationResponse(DataExchangeTerminalService dataExchangeTerminalService) : base(dataExchangeTerminalService)
    { }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;
    public override TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal) => throw new NotImplementedException();
    public override TerminalState Handle(TerminalSession session, OutReaderResponse signal) => throw new NotImplementedException();
    public override TerminalState Handle(TerminalSession session, QueryKernelResponse signal) => throw new NotImplementedException();

    public override TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal) =>
        throw new NotImplementedException();

    public override TerminalState Handle(TerminalSession? session, AcquirerResponseSignal signal) => throw new NotImplementedException();
    public override TerminalState Handle(TerminalSession session, QueryTerminalRequest signal) => throw new NotImplementedException();

    #endregion
}