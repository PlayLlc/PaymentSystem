using Play.Randoms;

namespace Play.Events;

internal readonly record struct InstanceId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public InstanceId()
    {
        _Value = Randomize.Integers.ULong();
    }

    #endregion

    #region Equality

    public bool Equals(InstanceId x, InstanceId y) => x._Value == y._Value;
    public int GetHashCode(InstanceId obj) => obj._Value.GetHashCode();

    #endregion
}