using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine;

public interface IGetKernelState
{
    public KernelState GetKernelState(StateId stateId);
}