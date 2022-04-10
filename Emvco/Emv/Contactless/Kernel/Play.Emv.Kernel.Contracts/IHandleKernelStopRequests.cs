namespace Play.Emv.Kernel.Contracts;

public interface IHandleKernelStopRequests
{
    #region Instance Members

    public void Request(StopKernelRequest message);

    #endregion
}