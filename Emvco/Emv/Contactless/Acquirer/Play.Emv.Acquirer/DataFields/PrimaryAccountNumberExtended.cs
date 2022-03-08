using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record PrimaryAccountNumberExtended : VariableDataField<byte[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 34</remarks>
    public static readonly DataFieldId DataFieldId = new(34);

    public static readonly PlayEncodingId EncodingId = NumericSpecialCodec.EncodingId;
    private const ushort _MaxByteCount = 28;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public PrimaryAccountNumberExtended(byte[] value) : base(value)
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
    public override PrimaryAccountNumberExtended Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<byte[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<byte[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PrimaryAccountNumberExtended(result.Value);
    }

    #endregion
}