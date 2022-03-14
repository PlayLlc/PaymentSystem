using System.Numerics;

using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record ExtendedBitmapIndicator : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 65</remarks>
    public static readonly DataFieldId DataFieldId = new(65);

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public ExtendedBitmapIndicator(BigInteger value) : base(value)
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
    public override ExtendedBitmapIndicator Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ExtendedBitmapIndicator(result.Value);
    }

    #endregion
}