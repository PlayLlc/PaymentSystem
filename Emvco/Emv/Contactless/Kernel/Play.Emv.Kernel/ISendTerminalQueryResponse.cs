using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel;

public interface ISendTerminalQueryResponse
{
    #region Instance Members

    public void Send(QueryKernelResponse message);

    #endregion
}