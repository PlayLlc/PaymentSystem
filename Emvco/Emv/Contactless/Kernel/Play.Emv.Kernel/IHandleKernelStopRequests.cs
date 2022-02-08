using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel;

public interface IHandleKernelStopRequests
{
    public void Request(StopKernelRequest message);
}