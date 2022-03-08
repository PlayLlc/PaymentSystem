using System.Numerics;

using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record NetSettlementAmount : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 97</remarks>
    public static readonly DataFieldId DataFieldId = new(97);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 16;

    #endregion

    #region Constructor

    public NetSettlementAmount(BigInteger value) : base(value)
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
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    /// <exception cref="InterchangeException"></exception>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    /// <exception cref="InterchangeDataFieldNullException"></exception>
    public override NetSettlementAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NetSettlementAmount(result.Value);
    }

    #endregion
}