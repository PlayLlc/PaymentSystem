using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record LocalTransactionDateMmdd : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 13</remarks>
    public static readonly DataFieldId DataFieldId = new(13);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public LocalTransactionDateMmdd(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override LocalTransactionDateMmdd Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new LocalTransactionDateMmdd(result.Value);
    }

    #endregion
}