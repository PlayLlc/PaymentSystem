namespace Play.Messaging;

internal record ResponseMessageEnvelope
{
    #region Instance Values

    protected readonly ResponseMessageHeader _MessageHeader;
    protected readonly ResponseMessage _Message;

    #endregion

    #region Constructor

    public ResponseMessageEnvelope(ResponseMessageHeader messageHeader, ResponseMessage message)
    {
        _MessageHeader = messageHeader;
        _Message = message;
    }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _MessageHeader.GetCorrelationId();
    public ResponseMessage GetMessage() => _Message;

    #endregion
}