namespace Play.Emv.Kernel2.Databases._Temp;

public interface ITlvDatabase : IQueryTlvDatabase, IWriteTlvDatabase
{
    #region Instance Members

    /// <summary>
    ///     Clears the database of transient values and restores the persistent and default values of the database
    /// </summary>
    public void Clear();

    #endregion
}