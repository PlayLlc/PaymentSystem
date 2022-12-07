namespace Play.Emv.Ber.ValueTypes;

/// <summary>
///     Indicates the text string to be displayed, with the different standard messages defined in  Book A section 9.4. If
///     the Message EncodingId is not recognized, the reader should ignore it and the message currently displayed should
///     not be changed as a result of the User Interface Request.
/// </summary>
public readonly record struct DisplayMessageIdentifier
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public DisplayMessageIdentifier(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Serialization

    public void Decode(Span<byte> buffer, ref int offset)
    {
        buffer[offset++] = _Value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator byte(DisplayMessageIdentifier value) => value._Value;
    public static bool operator ==(DisplayMessageIdentifier left, byte right) => left._Value == right;
    public static bool operator !=(DisplayMessageIdentifier left, byte right) => left._Value != right;
    public static bool operator ==(byte left, DisplayMessageIdentifier right) => left == right._Value;
    public static bool operator !=(byte left, DisplayMessageIdentifier right) => left != right._Value;

    #endregion
}