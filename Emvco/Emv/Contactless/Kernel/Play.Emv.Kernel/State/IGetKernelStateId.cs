using Play.Emv.Sessions;

namespace Play.Emv.Kernel.State;

public interface IGetKernelStateId
{
    public StateId GetStateId();
}