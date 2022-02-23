namespace Play.Codecs;

public record PlayEncodingId
{
    #region Instance Values

    private readonly int _Value;
    private readonly string _FullyQualifiedName;

    #endregion

    #region Constructor

    public PlayEncodingId(Type value)
    {
        _Value = PlayCodec.SignedIntegerCodec.DecodeToInt32(PlayCodec.UnicodeCodec.Encode(value.FullName));
        _FullyQualifiedName = value.FullName!;
    }

    #endregion

    #region Instance Members

    public string GetFullyQualifiedName() => _FullyQualifiedName;
    public override string ToString() => $"{_FullyQualifiedName}";

    #endregion

    #region Operator Overrides

    public static explicit operator int(PlayEncodingId value) => value._Value;

    #endregion
}