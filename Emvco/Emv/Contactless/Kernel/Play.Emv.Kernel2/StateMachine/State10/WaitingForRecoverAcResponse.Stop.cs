using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForRecoverAcResponse
{
    #region STOP

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        HandleRequestOutOfSync(session, signal);

        // CHECK: The S10 algorithm only specified to recursively set the state on a STOP signal. Check the specs to make sure that's valid with all STOP signal rules
        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion
}