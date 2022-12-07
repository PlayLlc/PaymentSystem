namespace Play.Events;

public readonly record struct EventTypeId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public EventTypeId(ulong value)
    {
        _Value = value;
    }

    #endregion

    #region Equality

    public bool Equals(EventTypeId x, EventTypeId y) => x._Value == y._Value;
    public int GetHashCode(EventTypeId obj) => obj._Value.GetHashCode();

    #endregion
}