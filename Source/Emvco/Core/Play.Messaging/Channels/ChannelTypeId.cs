using Play.Codecs;

namespace Play.Messaging;

public readonly record struct ChannelTypeId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public ChannelTypeId(string type)
    {
        _Value = 17;

        byte[] bytes = PlayCodec.AsciiCodec.Encode(type);

        for (int index = 0; index < bytes.Length; index++)
        {
            unchecked
            {
                _Value *= bytes[index];
            }
        }
    }

    #endregion

    #region Equality

    public bool Equals(ChannelTypeId x, ChannelTypeId y) => x._Value == y._Value;
    public int GetHashCode(ChannelTypeId obj) => obj._Value.GetHashCode();

    #endregion
}