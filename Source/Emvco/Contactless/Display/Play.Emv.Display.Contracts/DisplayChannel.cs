using Play.Messaging;

namespace Play.Emv.Display.Contracts;

public readonly record struct DisplayChannel
{
    #region Static Metadata

    public static readonly ChannelTypeId Id;

    #endregion

    #region Instance Values

    private readonly ChannelTypeId _Value;

    #endregion

    #region Constructor

    static DisplayChannel()
    {
        Id = new ChannelTypeId(nameof(DisplayChannel));
    }

    private DisplayChannel(ChannelTypeId value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ChannelTypeId(DisplayChannel value) => value._Value;

    #endregion
}