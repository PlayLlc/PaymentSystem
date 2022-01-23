namespace Play.Messaging;

public record MessageIdentifier : IEqualityComparer<MessageIdentifier>
{
    #region Instance Values

    private readonly InstanceId _InstanceId;
    private readonly MessageTypeId _MessageTypeId;
    private readonly ChannelTypeId _ChannelTypeId;

    #endregion

    #region Constructor

    public MessageIdentifier(MessageTypeId messageTypeId, ChannelTypeId channelTypeId)
    {
        _InstanceId = new InstanceId();
        _MessageTypeId = messageTypeId;
        _ChannelTypeId = channelTypeId;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     The identifier of the <see cref="IMessageChannel" /> that is responsible for handling this message
    /// </summary>
    /// <returns></returns>
    public ChannelTypeId GetChannelTypeId() => _ChannelTypeId;

    /// <summary>
    ///     The identifier of this message type
    /// </summary>
    /// <returns></returns>
    public MessageTypeId GetMessageTypeId() => _MessageTypeId;

    public CorrelationId GetCorrelationId() => new(this);

    #endregion

    #region Equality

    public bool Equals(MessageIdentifier? x, MessageIdentifier? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(MessageIdentifier obj) => obj.GetHashCode();

    #endregion
}