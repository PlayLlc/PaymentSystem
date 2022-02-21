using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record SystemTraceAuditNumberStan : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 11</remarks>
    public static readonly DataFieldId DataFieldId = new(11);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 3;

    #endregion

    #region Constructor

    public SystemTraceAuditNumberStan(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override SystemTraceAuditNumberStan Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new SystemTraceAuditNumberStan(result.Value);
    }

    #endregion
}