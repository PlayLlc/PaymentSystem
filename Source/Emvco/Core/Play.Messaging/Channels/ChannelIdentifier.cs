namespace Play.Messaging;

public record ChannelIdentifier
{
    #region Instance Values

    private readonly InstanceId _InstanceId;
    private readonly ChannelTypeId _ChannelTypeId;

    #endregion

    #region Constructor

    public ChannelIdentifier(ChannelTypeId channelTypeId)
    {
        _InstanceId = new InstanceId();
        _ChannelTypeId = channelTypeId;
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => _ChannelTypeId;

    #endregion
}