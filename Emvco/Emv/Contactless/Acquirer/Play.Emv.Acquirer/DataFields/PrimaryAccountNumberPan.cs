using System.Numerics;

using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record PrimaryAccountNumberPan : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 2</remarks>
    public static readonly DataFieldId DataFieldId = new(2);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 10;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public PrimaryAccountNumberPan(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override PrimaryAccountNumberPan Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PrimaryAccountNumberPan(result.Value);
    }

    #endregion
}