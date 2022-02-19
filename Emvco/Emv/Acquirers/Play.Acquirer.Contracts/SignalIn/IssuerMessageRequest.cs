namespace Play.Acquirer.Contracts;

public abstract record IssuerMessageRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(IssuerMessageRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Terminal;

    #endregion

    #region Instance Values

    public readonly MessageTypeIndicator MessageTypeIndicator;
    public readonly TagLengthValue[] TagLengthValues;

    #endregion

    #region Constructor

    protected IssuerMessageRequest(MessageTypeIndicator messageTypeIndicator, TagLengthValue[] tagLengthValues) : base(MessageTypeId,
        ChannelTypeId)
    {
        MessageTypeIndicator = messageTypeIndicator;
        TagLengthValues = tagLengthValues;
    }

    #endregion
}