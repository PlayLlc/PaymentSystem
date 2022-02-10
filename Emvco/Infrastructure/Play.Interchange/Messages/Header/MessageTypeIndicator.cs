namespace Play.Interchange.Messages.Header;

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