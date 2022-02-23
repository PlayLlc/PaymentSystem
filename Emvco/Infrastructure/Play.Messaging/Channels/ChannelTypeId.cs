namespace Play.Messaging;

public readonly record struct ChannelTypeId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public ChannelTypeId(string type)
    {
        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(PlayCodec.AsciiCodec.Encode(type));
    }

    #endregion

    #region Equality

    public bool Equals(ChannelTypeId x, ChannelTypeId y) => x._Value == y._Value;
    public int GetHashCode(ChannelTypeId obj) => obj._Value.GetHashCode();

    #endregion
}