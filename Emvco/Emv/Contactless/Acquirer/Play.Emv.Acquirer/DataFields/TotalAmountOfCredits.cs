using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record TotalAmountOfCredits : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 86</remarks>
    public static readonly DataFieldId DataFieldId = new(86);

    public static readonly PlayEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public TotalAmountOfCredits(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override TotalAmountOfCredits Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new TotalAmountOfCredits(result.Value);
    }

    #endregion
}