using Play.Emv.Identifiers;

namespace DeleteMe._Temp;

public interface IManageKernelDatabaseLifetime
{
    #region Instance Members

    public void Activate(KernelSessionId kernelSessionId);
    public void Deactivate();

    #endregion
}