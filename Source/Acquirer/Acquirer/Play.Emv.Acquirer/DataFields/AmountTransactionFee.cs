using System.Numerics;

using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record AmountTransactionFee : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 28</remarks>
    public static readonly DataFieldId DataFieldId = new(28);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public AmountTransactionFee(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    /// <exception cref="InterchangeException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InterchangeDataFieldNullException"></exception>
    public override AmountTransactionFee Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountTransactionFee(result.Value);
    }

    #endregion
}