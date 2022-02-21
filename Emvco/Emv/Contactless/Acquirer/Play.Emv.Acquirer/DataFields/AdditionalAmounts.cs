using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record AdditionalAmounts : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 54</remarks>
    public static readonly DataFieldId DataFieldId = new(54);

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const ushort _MaxByteCount = 120;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public AdditionalAmounts(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AdditionalAmounts Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AdditionalAmounts(result.Value);
    }

    #endregion
}