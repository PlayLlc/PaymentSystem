using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record ReplacementAmounts : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 95</remarks>
    public static readonly DataFieldId DataFieldId = new(95);

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const ushort _ByteCount = 42;

    #endregion

    #region Constructor

    public ReplacementAmounts(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ReplacementAmounts Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ReplacementAmounts(result.Value);
    }

    #endregion
}