using Play.Emv.Kernel.State;
using Play.Emv.Sessions;

namespace Play.Emv.Kernel2.StateMachine;

public interface IGetKernelState
{
    public KernelState GetKernelState(StateId stateId);
}