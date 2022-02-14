namespace Play.Interchange.Messages.Header;

/// <summary>
///     The message type indicator is a four-digit numeric field which indicates the overall function of the message. A
///     message type indicator includes the ISO 8583 version, the Message Class, the Message Function and the Message
///     Origin, as described below.
/// </summary>
public readonly ref struct MessageTypeIndicator
{
    #region Instance Values

    private readonly byte _FirstByte;
    private readonly byte _SecondByte;

    #endregion

    #region Constructor

    public MessageTypeIndicator(Version version, Class @class, Function function, Origin origin)
    {
        _FirstByte = 0;
        _SecondByte = 0;

        _FirstByte |= (byte) (version * 10);
        _FirstByte |= @class;

        _SecondByte |= (byte) (function * 10);
        _SecondByte |= origin;
    }

    #endregion

    #region Instance Members

    public byte GetFirstByte() => _FirstByte;
    public byte GetSecondByte() => _SecondByte;

    #endregion
}