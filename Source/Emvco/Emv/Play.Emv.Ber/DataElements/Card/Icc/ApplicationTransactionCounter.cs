using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Counter maintained by the application in the ICC (incrementing the ATC is managed by the ICC)
/// </summary>
public record ApplicationTransactionCounter : DataElement<ushort>, IEqualityComparer<ApplicationTransactionCounter>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F36;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public ApplicationTransactionCounter(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    /// <summary>
    ///     Returns the an Ascii encoded char array of this value's Numeric (BCD) digits
    /// </summary>
    /// <returns></returns>
    public char[] AsCharArray() => PlayCodec.NumericCodec.DecodeToChars(EncodeValue());

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationTransactionCounter Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ApplicationTransactionCounter Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationTransactionCounter Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new ApplicationTransactionCounter(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(ApplicationTransactionCounter? x, ApplicationTransactionCounter? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationTransactionCounter obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(ApplicationTransactionCounter value) => value._Value;

    #endregion
}