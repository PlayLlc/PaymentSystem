using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.Databases;

public interface IManageKernelDatabaseLifetime
{
    #region Instance Members

    public void Activate(KernelSessionId kernelSessionId);
    public void Deactivate();

    #endregion
}