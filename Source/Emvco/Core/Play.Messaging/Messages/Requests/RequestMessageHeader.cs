namespace Play.Messaging;

public record RequestMessageHeader
{
    #region Instance Values

    private readonly CorrelationId _Correlation;
    private readonly MessagingConfiguration _MessagingConfiguration;

    #endregion

    #region Constructor

    public RequestMessageHeader(CorrelationId correlationId)
    {
        _Correlation = correlationId;
        _MessagingConfiguration = new MessagingConfiguration(null);
    }

    public RequestMessageHeader(CorrelationId correlationId, MessagingConfiguration messagingConfiguration)
    {
        _Correlation = correlationId;
        _MessagingConfiguration = messagingConfiguration;
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => _Correlation.GetChannelTypeId();
    public CorrelationId GetCorrelationId() => _Correlation;
    public MessagingConfiguration GetMessagingConfiguration() => _MessagingConfiguration;

    #endregion
}