using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Services.DataExchange;

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