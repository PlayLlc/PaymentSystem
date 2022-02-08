namespace Play.Emv.Kernel.Contracts;

public interface IHandleKernelStopRequests
{
    public void Request(StopKernelRequest message);
}