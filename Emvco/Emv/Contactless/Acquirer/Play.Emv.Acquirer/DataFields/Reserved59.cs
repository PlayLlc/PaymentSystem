using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record Reserved59 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 59</remarks>
    public static readonly DataFieldId DataFieldId = new(59);

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved59(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    /// <summary>
    /// Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    public override Reserved59 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved59(result.Value);
    }

    #endregion
}