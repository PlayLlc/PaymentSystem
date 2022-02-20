using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record PointOfServiceCaptureCode : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 26</remarks>
    public static readonly DataFieldId DataFieldId = new(26);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public PointOfServiceCaptureCode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override PointOfServiceCaptureCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PointOfServiceCaptureCode(result.Value);
    }

    #endregion
}