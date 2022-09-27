using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the card data input capability of the Terminal and Reader.
/// </summary>
public record SecurityCapability : DataElement<byte>, IEqualityComparer<SecurityCapability>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF811F;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public SecurityCapability(byte value) : base(value)
    { }

    #endregion

    #region Serialization

    public static SecurityCapability Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override SecurityCapability Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static SecurityCapability Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);
        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new SecurityCapability(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(SecurityCapability? x, SecurityCapability? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(SecurityCapability obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(SecurityCapability value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}