namespace Play.Acquirer.Contracts;

public abstract record IssuerMessageResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(IssuerMessageResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Terminal;

    #endregion

    #region Instance Values

    public readonly MessageTypeIndicator MessageTypeIndicator;
    public readonly TagLengthValue[] TagLengthValues;

    #endregion

    #region Constructor

    protected IssuerMessageResponse(
        CorrelationId correlationId,
        MessageTypeIndicator messageTypeIndicator,
        TagLengthValue[] tagLengthValues) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        MessageTypeIndicator = messageTypeIndicator;
        TagLengthValues = tagLengthValues;
    }

    #endregion
}