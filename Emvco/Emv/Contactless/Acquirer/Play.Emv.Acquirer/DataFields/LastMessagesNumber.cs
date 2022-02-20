using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record LastMessagesNumber : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 72</remarks>
    public static readonly DataFieldId DataFieldId = new(72);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public LastMessagesNumber(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override LastMessagesNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new LastMessagesNumber(result.Value);
    }

    #endregion
}