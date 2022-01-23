using Play.Random;

namespace Play.Emv.Reader.Services;

public readonly record struct MainSessionId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public MainSessionId()
    {
        _Value = Randomize.Integers.ULong();
    }

    #endregion

    #region Equality

    public bool Equals(MainSessionId x, MainSessionId y) => x._Value == y._Value;
    public int GetHashCode(MainSessionId obj) => obj._Value.GetHashCode();

    #endregion
}