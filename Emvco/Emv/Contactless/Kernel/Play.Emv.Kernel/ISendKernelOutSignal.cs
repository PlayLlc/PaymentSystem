using Play.Emv.Kernel.Contracts.SignalOut;

namespace Play.Emv.Kernel;

public interface ISendKernelOutSignal
{
    public void Send(OutKernelResponse message);
}