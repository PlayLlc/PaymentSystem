using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record FileName : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 101</remarks>
    public static readonly DataFieldId DataFieldId = new(101);

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const ushort _MaxByteCount = 17;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public FileName(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override FileName Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new FileName(result.Value);
    }

    #endregion
}