using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record CurrencyCodeCardholderBilling : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 51</remarks>
    public static readonly DataFieldId DataFieldId = new(51);

    public static readonly PlayEncodingId EncodingId = AlphabeticDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public CurrencyCodeCardholderBilling(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CurrencyCodeCardholderBilling Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CurrencyCodeCardholderBilling(result.Value);
    }

    #endregion
}