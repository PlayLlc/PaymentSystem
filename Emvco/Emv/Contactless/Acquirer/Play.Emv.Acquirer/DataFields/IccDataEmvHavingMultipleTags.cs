using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Acquirer.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;

namespace Play.Emv.Acquirer.DataFields;

public record IccDataEmvHavingMultipleTags : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 55</remarks>
    public static readonly DataFieldId DataFieldId = new(55);

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public IccDataEmvHavingMultipleTags(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override IccDataEmvHavingMultipleTags Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new IccDataEmvHavingMultipleTags(result.Value);
    }

    #endregion
}