using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel;

public interface IHandleKernelStopRequests
{
    #region Instance Members

    public void Request(StopKernelRequest message);

    #endregion
}