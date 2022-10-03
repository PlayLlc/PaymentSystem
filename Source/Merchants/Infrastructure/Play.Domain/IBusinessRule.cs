namespace Play.Domain;

public interface IBusinessRule
{
    #region Instance Values

    string Message { get; }

    #endregion

    #region Instance Members

    bool IsBroken();

    #endregion
}