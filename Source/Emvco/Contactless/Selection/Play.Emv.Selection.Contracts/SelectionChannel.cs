using Play.Messaging;

namespace Play.Emv.Selection.Contracts;

public readonly record struct SelectionChannel
{
    #region Static Metadata

    public static readonly ChannelTypeId Id;

    #endregion

    #region Instance Values

    private readonly ChannelTypeId _Value;

    #endregion

    #region Constructor

    static SelectionChannel()
    {
        Id = new ChannelTypeId(nameof(SelectionChannel));
    }

    private SelectionChannel(ChannelTypeId value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ChannelTypeId(SelectionChannel value) => value._Value;

    #endregion
}