using System.Numerics;

using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record AmountSettlementFee : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 29</remarks>
    public static readonly DataFieldId DataFieldId = new(29);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public AmountSettlementFee(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountSettlementFee Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountSettlementFee(result.Value);
    }

    #endregion
}