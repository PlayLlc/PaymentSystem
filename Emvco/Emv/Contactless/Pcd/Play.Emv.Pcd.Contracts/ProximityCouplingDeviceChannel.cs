using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public readonly record struct ProximityCouplingDeviceChannel
{
    #region Static Metadata

    public static readonly ChannelTypeId Id;

    #endregion

    #region Instance Values

    private readonly ChannelTypeId _Value;

    #endregion

    #region Constructor

    static ProximityCouplingDeviceChannel()
    {
        Id = new ChannelTypeId(nameof(ProximityCouplingDeviceChannel));
    }

    private ProximityCouplingDeviceChannel(ChannelTypeId value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ChannelTypeId(ProximityCouplingDeviceChannel value) => value._Value;

    #endregion
}