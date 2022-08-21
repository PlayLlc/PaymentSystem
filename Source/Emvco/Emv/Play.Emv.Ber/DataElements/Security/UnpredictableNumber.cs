using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Value to provide variability and uniqueness to the generation of a cryptogram
/// </summary>
public record UnpredictableNumber : DataElement<uint>, IEqualityComparer<UnpredictableNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F37;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public UnpredictableNumber(uint value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static UnpredictableNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override UnpredictableNumber Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static UnpredictableNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new UnpredictableNumber(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(UnpredictableNumber? x, UnpredictableNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UnpredictableNumber obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator uint(UnpredictableNumber value) => value._Value;

    #endregion

    #region Instance Members

    internal Nibble[] GetDigits()
    {
        byte digitsToCopy = (byte) GetSetBitCount();
        Nibble[] result = new Nibble[3];

        for (int i = result.Length - 1; i > 0; i--)
        {
            result[i] = (byte) (digitsToCopy % 10);
            digitsToCopy /= 10;
        }

        return result;
    }

    public ushort GetByteCount() => _ByteLength;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public int GetSetBitCount() => _Value.GetSetBitCount();

    #endregion
}