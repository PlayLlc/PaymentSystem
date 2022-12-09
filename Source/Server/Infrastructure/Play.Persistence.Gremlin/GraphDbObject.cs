namespace Play.Persistence.Gremlin;

public abstract record GraphDbObject
{
    #region Instance Members

    public static string GetLabel(dynamic obj) => obj.GetType().Name;

    #endregion
}