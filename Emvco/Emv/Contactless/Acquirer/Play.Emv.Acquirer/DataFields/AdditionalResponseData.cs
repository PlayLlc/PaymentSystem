using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record AdditionalResponseData : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 44</remarks>
    public static readonly DataFieldId DataFieldId = new(44);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 25;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public AdditionalResponseData(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AdditionalResponseData Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AdditionalResponseData(result.Value);
    }

    #endregion
}