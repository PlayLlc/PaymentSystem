using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public record UnpredictableNumberNumeric : DataElement<uint>, IEqualityComparer<UnpredictableNumberNumeric>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F6A;
    private const byte _ByteLength = 4;
    private const byte _CharLength = 8;

    #endregion

    #region Constructor

    public UnpredictableNumberNumeric(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int GetSetBitCount() => _Value.GetSetBitCount();

    /// <summary>
    ///     Returns the an Ascii encoded char array of this value's Numeric (BCD) digits
    /// </summary>
    /// <returns></returns>
    public char[] AsCharArray() => PlayCodec.NumericCodec.DecodeToChars(EncodeValue());

    internal Nibble[] GetDigits()
    {
        uint valueBuffer = _Value;
        Nibble[] result = new Nibble[3];

        for (int i = result.Length - 1; i > 0; i--)
        {
            result[i] = (byte) (valueBuffer % 10);
            valueBuffer /= 10;
        }

        return result;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static UnpredictableNumberNumeric Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static UnpredictableNumberNumeric Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.NumericCodec.DecodeToUInt32(value);

        Check.Primitive.ForCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new UnpredictableNumberNumeric(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(UnpredictableNumberNumeric? x, UnpredictableNumberNumeric? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UnpredictableNumberNumeric obj) => obj.GetHashCode();

    #endregion
}