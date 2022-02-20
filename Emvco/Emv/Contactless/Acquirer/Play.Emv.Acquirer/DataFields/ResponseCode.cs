using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record ResponseCode : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 39</remarks>
    public static readonly DataFieldId DataFieldId = new(39);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public ResponseCode(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ResponseCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ResponseCode(result.Value);
    }

    #endregion
}