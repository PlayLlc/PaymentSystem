using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record Reserved58 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 58</remarks>
    public static readonly DataFieldId DataFieldId = new(58);

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved58(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved58 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved58(result.Value);
    }

    #endregion
}