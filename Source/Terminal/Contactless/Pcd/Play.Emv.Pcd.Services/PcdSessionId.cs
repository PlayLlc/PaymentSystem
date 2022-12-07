using Play.Randoms;

namespace Play.Emv.Pcd.Services;

public readonly record struct PcdSessionId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public PcdSessionId()
    {
        _Value = Randomize.Integers.ULong();
    }

    #endregion

    #region Equality

    public bool Equals(PcdSessionId x, PcdSessionId y) => x._Value == y._Value;
    public int GetHashCode(PcdSessionId obj) => obj._Value.GetHashCode();

    #endregion
}