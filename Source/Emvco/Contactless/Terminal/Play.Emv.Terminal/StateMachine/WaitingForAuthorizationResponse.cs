using System;

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

/// <summary>
///     This state transition handles Terminal requests when it is in the process of communicating with the Acquirer
/// </summary>
public class WaitingForAuthorizationResponse : TerminalState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForAuthorizationResponse));

    #endregion

    #region Constructor

    public WaitingForAuthorizationResponse(
        TerminalConfiguration terminalConfiguration, DataExchangeTerminalService dataExchangeTerminalService, IEndpointClient endpointClient,
        IGetTerminalState terminalStateResolver) : base(dataExchangeTerminalService, terminalConfiguration, endpointClient, terminalStateResolver)
    { }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;
    public override TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal) => throw new NotImplementedException();
    public override TerminalState Handle(TerminalSession session, OutReaderResponse signal) => throw new NotImplementedException();
    public override TerminalState Handle(TerminalSession session, QueryKernelResponse signal) => throw new NotImplementedException();
    public override TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal) => throw new NotImplementedException();

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession? session, ReconciliationResponse signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    public override TerminalState Handle(TerminalSession session, QueryTerminalRequest signal) => throw new NotImplementedException();

    #endregion
}