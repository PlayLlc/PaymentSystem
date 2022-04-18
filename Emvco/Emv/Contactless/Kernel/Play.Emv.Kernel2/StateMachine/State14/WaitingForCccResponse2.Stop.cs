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

        return _KernelStateResolver.GetKernelState(StateId);
    }
}