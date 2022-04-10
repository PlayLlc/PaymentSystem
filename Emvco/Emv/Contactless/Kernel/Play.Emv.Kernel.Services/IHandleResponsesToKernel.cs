using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.Services;

public interface IHandleResponsesToKernel : ISendKernelOutSignal
{
    #region Instance Members

    public void Handle(QueryPcdResponse message);
    public void Handle(QueryTerminalResponse message);

    #endregion
}