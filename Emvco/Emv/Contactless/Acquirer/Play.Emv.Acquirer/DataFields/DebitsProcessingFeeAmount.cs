using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record DebitsProcessingFeeAmount : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 84</remarks>
    public static readonly DataFieldId DataFieldId = new(84);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public DebitsProcessingFeeAmount(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override DebitsProcessingFeeAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new DebitsProcessingFeeAmount(result.Value);
    }

    #endregion
}