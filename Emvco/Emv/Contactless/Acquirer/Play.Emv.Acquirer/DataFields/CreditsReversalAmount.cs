using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record CreditsReversalAmount : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 87</remarks>
    public static readonly DataFieldId DataFieldId = new(87);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public CreditsReversalAmount(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CreditsReversalAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CreditsReversalAmount(result.Value);
    }

    #endregion
}