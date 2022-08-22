using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Configuration;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.DataExchange;
using Play.Emv.Terminal.Session;
using Play.Messaging;

namespace Play.Emv.Terminal.StateMachine;

public abstract class TerminalState
{
    #region Instance Values

    protected readonly DataExchangeTerminalService _DataExchangeTerminalService;
    protected readonly TerminalConfiguration _TerminalConfiguration;
    protected readonly IEndpointClient _EndpointClient;
    protected readonly IGetTerminalState _TerminalStateResolver;

    #endregion

    #region Constructor

    protected TerminalState(
        DataExchangeTerminalService dataExchangeTerminalService, TerminalConfiguration terminalConfiguration, IEndpointClient endpointClient,
        IGetTerminalState terminalStateResolver)
    {
        _DataExchangeTerminalService = dataExchangeTerminalService;
        _TerminalConfiguration = terminalConfiguration;
        _EndpointClient = endpointClient;
        _TerminalStateResolver = terminalStateResolver;
    }

    #endregion

    #region Instance Members

    public abstract StateId GetStateId();
    public abstract TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal);
    public abstract TerminalState Handle(TerminalSession session, OutReaderResponse signal);
    public abstract TerminalState Handle(TerminalSession session, QueryKernelResponse signal);
    public abstract TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal);
    public abstract TerminalState Handle(TerminalSession? session, ReconciliationResponse signal);
    public abstract TerminalState Handle(TerminalSession session, QueryTerminalRequest signal);

    public void Clear()
    {
        _DataExchangeTerminalService.Clear();
    }

    #endregion
}