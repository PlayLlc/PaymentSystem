using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForEmvModeFirstWriteFlag : KernelState
{
    #region DET

    // BUG: Need to make sure you're properly implementing each DEK handler for each state
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleTimeout(session))
            return _KernelStateResolver.GetKernelState(StateId);

        UpdateDatabase(signal);

        // S6.7
        session.Timer.Stop();

        AttemptToHandleGetDataToBeDone(session.GetTransactionSessionId());

        return _KernelStateResolver.GetKernelState(_S456.Process(this, (Kernel2Session) session, signal));
    }

    #region S6.1, S6.3

    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section S6.1, S6.3</remarks>
    /// <exception cref="System.InvalidOperationException"></exception>
    public bool TryHandleTimeout(KernelSession session)
    {
        if (!session.Timer.IsTimedOut())
            return false;

        _Database.Update(StatusOutcome.EndApplication);
        _Database.Update(Level3Error.TimeOut);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region S6.6

    /// <remarks>Book C-2 Section S6.6</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateDatabase(QueryTerminalResponse signal)
    {
        _Database.Update(signal.GetDataToSend().AsPrimitiveValues());
    }

    #endregion

    #region S6.7

    // Implemented in the main QueryTerminalResponse Handle method

    #endregion

    #region S6.8 - S6.12

    /// <remarks>Book C-2 Section S6.8 - S6.12</remarks>
    /// <exception cref="System.InvalidOperationException"></exception>
    public bool AttemptToHandleGetDataToBeDone(TransactionSessionId sessionId)
    {
        if (!_DataExchangeKernelService.TryPeek(DekRequestType.TagsToRead, out Tag tagToRead))
            return false;

        _PcdEndpoint.Request(GetDataRequest.Create(tagToRead, sessionId));

        return true;
    }

    #endregion

    #endregion
}