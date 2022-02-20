using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record ConversionRateCardholderBilling : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 10</remarks>
    public static readonly DataFieldId DataFieldId = new(10);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 4;

    #endregion

    #region Constructor

    public ConversionRateCardholderBilling(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ConversionRateCardholderBilling Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ConversionRateCardholderBilling(result.Value);
    }

    #endregion
}