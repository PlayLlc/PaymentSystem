using Play.Emv.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse1
{
    #region Instance Members

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        // CHECK: The S9 algorithm only specified to recursively set the state on a STOP signal. Check the specs to make sure that's valid with all STOP signal rules
        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion
}