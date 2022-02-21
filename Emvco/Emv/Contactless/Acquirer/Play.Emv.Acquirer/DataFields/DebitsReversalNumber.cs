using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record DebitsReversalNumber : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 77</remarks>
    public static readonly DataFieldId DataFieldId = new(77);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public DebitsReversalNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override DebitsReversalNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new DebitsReversalNumber(result.Value);
    }

    #endregion
}