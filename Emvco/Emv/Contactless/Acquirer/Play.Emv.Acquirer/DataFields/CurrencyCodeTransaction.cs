using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record CurrencyCodeTransaction : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 49</remarks>
    public static readonly DataFieldId DataFieldId = new(49);

    public static readonly InterchangeEncodingId EncodingId = AlphabeticDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public CurrencyCodeTransaction(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CurrencyCodeTransaction Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CurrencyCodeTransaction(result.Value);
    }

    #endregion
}