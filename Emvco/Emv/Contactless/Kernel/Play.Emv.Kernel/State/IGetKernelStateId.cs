using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.State;

public interface IGetKernelStateId
{
    public StateId GetStateId();
}