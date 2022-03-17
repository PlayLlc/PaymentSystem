using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.Databases;

public interface IActivateKernelDatabase
{
    #region Instance Members

    public void Activate(KernelSessionId kernelSessionId, Transaction transaction);

    #endregion
}