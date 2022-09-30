using Play.Randoms;

namespace Play.Messaging;

internal readonly record struct MessageInstanceId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public MessageInstanceId()
    {
        _Value = Randomize.Integers.ULong();
    }

    #endregion

    #region Equality

    public bool Equals(MessageInstanceId x, MessageInstanceId y) => x._Value == y._Value;
    public int GetHashCode(MessageInstanceId obj) => obj._Value.GetHashCode();

    #endregion
}