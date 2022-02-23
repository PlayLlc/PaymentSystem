using System.Numerics;

using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record Track3Data : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 36</remarks>
    public static readonly DataFieldId DataFieldId = new(36);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _MaxByteCount = 52;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Track3Data(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Track3Data Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Track3Data(result.Value);
    }

    #endregion
}