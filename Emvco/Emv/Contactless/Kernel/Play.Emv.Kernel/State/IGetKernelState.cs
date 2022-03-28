using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.State;

public interface IGetKernelState
{
    public KernelState GetKernelState(StateId stateId);
}