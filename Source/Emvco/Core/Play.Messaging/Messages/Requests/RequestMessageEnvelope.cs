namespace Play.Messaging;

public record RequestMessageEnvelope
{
    #region Instance Values

    protected readonly RequestMessageHeader _RequestMessageHeader;
    protected readonly RequestMessage _Message;

    #endregion

    #region Constructor

    internal RequestMessageEnvelope(RequestMessageHeader requestMessageHeader, RequestMessage message)
    {
        _RequestMessageHeader = requestMessageHeader;
        _Message = message;
    }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _Message.GetCorrelationId();
    public ChannelTypeId GetChannelTypeId() => _RequestMessageHeader.GetChannelTypeId();
    public MessageTypeId GetMessageTypeId() => _Message.CreateMessageTypeId();
    public MessageIdentifier GetMessageIdentifier() => _Message.GetMessageIdentifier();
    public RequestMessage GetMessage() => _Message;

    #endregion
}