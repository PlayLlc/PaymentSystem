using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel;

public interface ISendKernelOutSignal
{
    #region Instance Members

    public void Send(OutKernelResponse message);

    #endregion
}