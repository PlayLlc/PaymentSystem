namespace Play.Emv.Acquirer.Contracts;

/// <summary>
///     The message type indicator is a four-digit numeric field which indicates the overall function of the message. A
///     message type indicator includes the ISO 8583 version, the Message Class, the Message Function and the Message
///     Origin, as described below.
/// </summary>
public readonly struct MessageTypeIndicator
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public MessageTypeIndicator(ushort value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ushort(MessageTypeIndicator value) => value._Value;

    #endregion
}