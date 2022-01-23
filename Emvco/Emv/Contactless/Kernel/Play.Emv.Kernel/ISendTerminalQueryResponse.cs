using Play.Emv.Kernel.Contracts.SignalOut;

namespace Play.Emv.Kernel;

public interface ISendTerminalQueryResponse
{
    public void Send(QueryKernelResponse message);
}