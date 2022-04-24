namespace Play.Messaging;

public record CorrelationId : IEqualityComparer<CorrelationId>
{
    #region Instance Values

    /// <summary>
    ///     The unique identifier of the current message and contains details on the message channel that handles this message
    ///     type
    /// </summary>
    private readonly MessageIdentifier _MessageIdentifier;

    #endregion

    #region Constructor

    public CorrelationId(MessageIdentifier messageIdentifier)
    {
        _MessageIdentifier = messageIdentifier;
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => _MessageIdentifier.GetChannelTypeId();
    public MessageTypeId GetMessageTypeId() => _MessageIdentifier.GetMessageTypeId();

    #endregion

    #region Equality

    public bool Equals(CorrelationId? x, CorrelationId? y)
    {
        if (x == null)
            return y == null;
        if (y == null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CorrelationId obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator CorrelationId(MessageIdentifier value) => new(value);

    #endregion
}