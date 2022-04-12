using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    #region Instance Members

    // BUG: Need to make sure you're properly implementing each DEK handler for each state

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        UpdateDatabase(signal);

        return _KernelStateResolver.GetKernelState(StateId);
    }

    /// <exception cref="TerminalDataException"></exception>
    private void UpdateDatabase(QueryTerminalResponse signal)
    {
        _Database.Update(signal.GetDataToSend().AsPrimitiveValues());
    }

    #endregion
}