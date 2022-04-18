using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForCccResponse2
{
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        HandleRequestOutOfSync(session, signal);

        // CHECK: The S9 algorithm only specified to recursively set the state on a STOP signal. Check the specs to make sure that's valid with all STOP signal rules
        return _KernelStateResolver.GetKernelState(StateId);
    }
}