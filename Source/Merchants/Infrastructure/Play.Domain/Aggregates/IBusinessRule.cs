using Play.Domain.Events;

namespace Play.Domain.Aggregates;

public interface IBusinessRule
{
    #region Instance Values

    string Message { get; }

    #endregion

    #region Instance Members

    public bool IsBroken();

    #endregion
}