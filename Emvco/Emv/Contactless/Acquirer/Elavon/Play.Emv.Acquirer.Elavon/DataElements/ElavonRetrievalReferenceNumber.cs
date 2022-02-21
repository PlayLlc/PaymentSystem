namespace Play.Emv.Acquirer.Elavon.DataElements;

/// <summary>
///     The Elavon RRN value will be returned to the POS or partner host system in the event of an approval of a sale
///     transaction. This data must be provided to the Elavon host in the event of a reversal of the aforementioned sale
///     transaction. This sub-field must be populated irrespective of whether single or dual message processing is used
/// </summary>
public record ElavonRetrievalReferenceNumber : ElavonDataElement<char[]>
{
    #region Static Metadata

    /// <value>Hex: 0x0004 Decimal: 4</value>
    public static readonly Tag Tag = 0x0004;

    public static readonly BerEncodingId BerEncodingId = AlphaNumericCodec.Identifier;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public ElavonRetrievalReferenceNumber(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ElavonRetrievalReferenceNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public static ElavonRetrievalReferenceNumber Decode(ReadOnlySpan<byte> value)
    {
        const ushort charLength = 12;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<char[]> result = _Codec.Decode(AlphaNumericCodec.Identifier, value) as DecodedResult<char[]>
            ?? throw new DataElementNullException(BerEncodingId);

        Check.Primitive.ForCharLength(result.CharCount, charLength, Tag);

        return new ElavonRetrievalReferenceNumber(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(ElavonRetrievalReferenceNumber? x, ElavonRetrievalReferenceNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ElavonRetrievalReferenceNumber obj) => obj.GetHashCode();

    #endregion
}