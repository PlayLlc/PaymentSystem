namespace Play.Messaging;

public record RequestMessageHeader
{
    #region Instance Values

    private readonly CorrelationId _Correlation;

    #endregion

    #region Constructor

    public RequestMessageHeader(CorrelationId correlationId)
    {
        _Correlation = correlationId;
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => _Correlation.GetChannelTypeId();
    public CorrelationId GetCorrelationId() => _Correlation;

    #endregion
}