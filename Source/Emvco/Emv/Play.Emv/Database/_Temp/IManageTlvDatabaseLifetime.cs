using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.Databases;

public interface IManageTlvDatabaseLifetime
{
    #region Instance Members

    public void Activate(TransactionSessionId kernelSessionId);
    public void Deactivate();

    #endregion
}