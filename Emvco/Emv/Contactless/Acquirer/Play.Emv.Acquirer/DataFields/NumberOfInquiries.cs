using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record NumberOfInquiries : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 80</remarks>
    public static readonly DataFieldId DataFieldId = new(80);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public NumberOfInquiries(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override NumberOfInquiries Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NumberOfInquiries(result.Value);
    }

    #endregion
}