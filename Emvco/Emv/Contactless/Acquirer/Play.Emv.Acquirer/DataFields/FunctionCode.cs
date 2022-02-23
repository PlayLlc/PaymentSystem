using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record FunctionCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 24</remarks>
    public static readonly DataFieldId DataFieldId = new(24);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public FunctionCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override FunctionCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new FunctionCode(result.Value);
    }

    #endregion
}