using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.Services;

public abstract class KernelStateMachine
{
    #region Instance Members

    public abstract void Handle(ActivateKernelRequest signal);
    public abstract void Handle(CleanKernelRequest signal);
    public abstract void Handle(QueryKernelRequest signal);
    public abstract void Handle(StopKernelRequest signal);
    public abstract void Handle(UpdateKernelRequest signal);
    public abstract void Handle(QueryPcdResponse signal);
    public abstract void Handle(QueryTerminalResponse signal);

    #endregion
}