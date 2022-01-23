using Play.Randoms;

namespace Play.Emv.Terminal.Services;

internal readonly record struct TerminalSessionId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public TerminalSessionId()
    {
        _Value = Randomize.Integers.ULong();
    }

    #endregion

    #region Equality

    public bool Equals(TerminalSessionId x, TerminalSessionId y)
    {
        return x._Value == y._Value;
    }

    public int GetHashCode(TerminalSessionId obj)
    {
        return obj._Value.GetHashCode();
    }

    #endregion
}