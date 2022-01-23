using Play.Randoms;

namespace Play.Messaging;

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

    public bool Equals(InstanceId x, InstanceId y)
    {
        return x._Value == y._Value;
    }

    public int GetHashCode(InstanceId obj)
    {
        return obj._Value.GetHashCode();
    }

    #endregion
}