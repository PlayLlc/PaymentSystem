namespace Play.Emv.Acquirers.Elavon.DataElements;

/// <summary>
///     The Elavon Void Item number will only be populated in single message operation. The Elavon Void Item number must be
///     provided to the Elavon host in the event of the reversal of a previously authorized sale (1200), refund or force
///     (1220) transaction. This sub-field is only required in reversal requests for single message implementations where
///     the Elavon acquiring  host captures transactions for settlement.
/// </summary>
public record ElavonVoidItemNumber : ElavonDataElement<ushort>
{
    #region Static Metadata

    /// <value>Hex: 0x0005 Decimal: 5</value>
    public static readonly Tag Tag = 0x0005;

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public ElavonVoidItemNumber(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ElavonVoidItemNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public static ElavonVoidItemNumber Decode(ReadOnlySpan<byte> value)
    {
        const ushort charLength = 3;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(NumericCodec.Identifier, value).ToUInt16Result()
            ?? throw new DataElementNullException(BerEncodingId);

        Check.Primitive.ForMaxCharLength(result.Value.GetNumberOfDigits(), charLength, Tag);

        return new ElavonVoidItemNumber(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(ElavonVoidItemNumber? x, ElavonVoidItemNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ElavonVoidItemNumber obj) => obj.GetHashCode();

    #endregion
}