using Play.Emv.Sessions;

namespace Play.Emv.Kernel.Databases;

public interface IActivateKernelDatabase
{
    #region Instance Members

    public void Activate(KernelSessionId kernelSessionId, Transaction transaction);

    #endregion
}