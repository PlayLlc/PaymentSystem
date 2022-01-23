using Play.Emv.Kernel.Contracts.SignalIn;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.State;

public abstract class KernelState
{
    #region Instance Members

    public abstract KernelStateId GetKernelStateId();
    public abstract KernelState Handle(ActivateKernelRequest signal);
    public abstract KernelState Handle(CleanKernelRequest signal);
    public abstract KernelState Handle(QueryKernelRequest signal);
    public abstract KernelState Handle(StopKernelRequest signal);
    public abstract KernelState Handle(UpdateKernelRequest signal);
    public abstract KernelState Handle(QueryPcdResponse signal);
    public abstract KernelState Handle(QueryTerminalResponse signal);

    #endregion
}