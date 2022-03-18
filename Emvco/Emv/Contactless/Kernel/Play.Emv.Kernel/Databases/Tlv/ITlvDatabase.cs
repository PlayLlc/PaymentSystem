using Play.Emv.Database;

namespace Play.Emv.Kernel.Databases.Tlv;

public interface ITlvDatabase : IQueryTlvDatabase, IWriteTlvDatabase
{
    #region Instance Members

    /// <summary>
    ///     Clears the database of transient values and restores the persistent and default values of the database
    /// </summary>
    public void Clear();

    #endregion
}