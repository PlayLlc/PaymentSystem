using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.DataExchange;
using Play.Emv.Terminal.Session;

namespace Play.Emv.Terminal.StateMachine;

public abstract class TerminalState
{
    #region Instance Values

    protected readonly DataExchangeTerminalService _DataExchangeTerminalService;

    #endregion

    #region Constructor

    protected TerminalState(DataExchangeTerminalService dataExchangeTerminalService)
    {
        _DataExchangeTerminalService = dataExchangeTerminalService;
    }

    #endregion

    #region Instance Members

    public abstract StateId GetStateId();
    public abstract TerminalState Handle(TerminalSession? session, InitiateSettlementRequest signal);
    public abstract TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal);
    public abstract TerminalState Handle(TerminalSession session, OutReaderResponse signal);
    public abstract TerminalState Handle(TerminalSession session, QueryKernelResponse signal);
    public abstract TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal);
    public abstract TerminalState Handle(TerminalSession? session, AcquirerResponseSignal signal);
    public abstract TerminalState Handle(TerminalSession session, QueryTerminalRequest signal);

    public void Clear()
    {
        _DataExchangeTerminalService.Clear();
    }

    #endregion
}