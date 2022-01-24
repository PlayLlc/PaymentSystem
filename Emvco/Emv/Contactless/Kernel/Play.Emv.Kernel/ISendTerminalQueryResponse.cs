using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel;

public interface ISendTerminalQueryResponse
{
    public void Send(QueryKernelResponse message);
}