using Play.Random;

namespace Play.Emv.Selection.Services;

public readonly record struct SelectionSessionId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public SelectionSessionId()
    {
        _Value = Randomize.Integers.ULong();
    }

    #endregion

    #region Equality

    public bool Equals(SelectionSessionId x, SelectionSessionId y) => x._Value == y._Value;
    public int GetHashCode(SelectionSessionId obj) => obj._Value.GetHashCode();

    #endregion
}