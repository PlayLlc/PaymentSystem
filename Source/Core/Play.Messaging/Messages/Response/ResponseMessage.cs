namespace Play.Messaging;

public abstract record ResponseMessage : Message
{
    #region Instance Values

    private readonly CorrelationId _CorrelationId;

    #endregion

    #region Constructor

    /// <summary>
    /// </summary>
    /// <param name="correlationId">
    ///     The <see cref="CorrelationId" /> constructed from the the original <see cref="RequestMessage" />
    /// </param>
    /// <param name="messageTypeId"></param>
    /// <param name="channelTypeId"></param>
    protected ResponseMessage(CorrelationId correlationId, MessageTypeId messageTypeId, ChannelTypeId channelTypeId) : base(messageTypeId, channelTypeId)
    {
        _CorrelationId = correlationId;
    }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _CorrelationId;

    #endregion
}