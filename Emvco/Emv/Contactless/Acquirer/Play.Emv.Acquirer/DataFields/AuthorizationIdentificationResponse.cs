using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record AuthorizationIdentificationResponse : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 38</remarks>
    public static readonly DataFieldId DataFieldId = new(38);

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public AuthorizationIdentificationResponse(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AuthorizationIdentificationResponse Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AuthorizationIdentificationResponse(result.Value);
    }

    #endregion
}