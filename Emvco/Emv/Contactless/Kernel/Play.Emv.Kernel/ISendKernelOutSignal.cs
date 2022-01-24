using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel;

public interface ISendKernelOutSignal
{
    public void Send(OutKernelResponse message);
}