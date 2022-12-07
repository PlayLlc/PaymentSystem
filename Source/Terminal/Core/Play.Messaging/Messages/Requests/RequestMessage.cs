namespace Play.Messaging;

public abstract record RequestMessage : Message
{
    #region Constructor

    protected RequestMessage(ChannelTypeId channelTypeId, MessageTypeId messageTypeId) : base(messageTypeId, channelTypeId)
    { }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _MessageIdentifier.GetCorrelationId();

    #endregion
}