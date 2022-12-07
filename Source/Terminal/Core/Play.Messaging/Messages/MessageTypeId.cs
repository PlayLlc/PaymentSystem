namespace Play.Messaging;

public readonly record struct MessageTypeId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public MessageTypeId(ulong value)
    {
        _Value = value;
    }

    #endregion

    #region Equality

    public bool Equals(MessageTypeId x, MessageTypeId y) => x._Value == y._Value;
    public int GetHashCode(MessageTypeId obj) => obj._Value.GetHashCode();

    #endregion
}