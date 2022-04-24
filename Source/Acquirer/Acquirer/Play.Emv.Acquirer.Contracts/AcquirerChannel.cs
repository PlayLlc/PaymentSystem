using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts;

public readonly record struct AcquirerChannel
{
    #region Static Metadata

    public static readonly ChannelTypeId Id;

    #endregion

    #region Instance Values

    private readonly ChannelTypeId _Value;

    #endregion

    #region Constructor

    static AcquirerChannel()
    {
        Id = new ChannelTypeId(nameof(AcquirerChannel));
    }

    private AcquirerChannel(ChannelTypeId value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ChannelTypeId(AcquirerChannel value) => value._Value;

    #endregion
}