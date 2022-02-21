namespace Play.Emv.Acquirer.Elavon.DataElements;

/// <summary>
///     Generic info needed for foreign currency exchange
/// </summary>
public record ElavonForexGenericInfo : ElavonDataElement<ushort>
{
    #region Static Metadata

    /// <value>Hex: 0x0011 Decimal: 17</value>
    public static readonly Tag Tag = 0x0011;

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const byte _ByteLength = 10;

    #endregion

    #region Constructor

    public ElavonForexGenericInfo(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ElavonForexGenericInfo Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public static ElavonForexGenericInfo Decode(ReadOnlySpan<byte> value)
    {
        const ushort charLength = 20;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(AlphaNumericCodec.Identifier, value).ToUInt16Result()
            ?? throw new DataElementNullException(BerEncodingId);

        Check.Primitive.ForMaxCharLength(result.Value.GetNumberOfDigits(), charLength, Tag);

        return new ElavonForexGenericInfo(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(ElavonForexGenericInfo? x, ElavonForexGenericInfo? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ElavonForexGenericInfo obj) => obj.GetHashCode();

    #endregion
}