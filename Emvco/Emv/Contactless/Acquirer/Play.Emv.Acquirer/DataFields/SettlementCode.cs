using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record SettlementCode : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 66</remarks>
    public static readonly DataFieldId DataFieldId = new(66);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public SettlementCode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override SettlementCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new SettlementCode(result.Value);
    }

    #endregion
}