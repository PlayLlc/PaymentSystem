using Play.Randoms;

namespace Play.Events;

internal readonly record struct EventId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public EventId()
    {
        _Value = Randomize.Integers.ULong();
    }

    #endregion

    #region Equality

    public bool Equals(EventId x, EventId y) => x._Value == y._Value;
    public int GetHashCode(EventId obj) => obj._Value.GetHashCode();

    #endregion
}