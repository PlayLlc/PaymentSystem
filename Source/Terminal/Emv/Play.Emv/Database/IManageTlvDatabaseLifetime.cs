using Play.Emv.Identifiers;

namespace Play.Emv.Database;

public interface IManageTlvDatabaseLifetime
{
    #region Instance Members

    public void Activate(TransactionSessionId kernelSessionId);
    public void Deactivate();

    #endregion
}