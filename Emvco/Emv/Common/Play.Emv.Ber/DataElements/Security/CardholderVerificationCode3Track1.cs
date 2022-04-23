using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: The CVC3 (Track1) is a 2-byte cryptogram returned by the Card in the response to the COMPUTE
///     CRYPTOGRAPHIC CHECKSUM command.
/// </summary>
public record CardholderVerificationCode3Track1 : DataElement<ushort>, IEqualityComparer<CardholderVerificationCode3Track1>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F60;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public CardholderVerificationCode3Track1(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public char[] AsCharArray() => PlayCodec.NumericCodec.DecodeToChars(EncodeValue());

    public static bool EqualsStatic(CardholderVerificationCode3Track1? x, CardholderVerificationCode3Track1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static CardholderVerificationCode3Track1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override CardholderVerificationCode3Track1 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static CardholderVerificationCode3Track1 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);
        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new CardholderVerificationCode3Track1(result);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(CardholderVerificationCode3Track1? x, CardholderVerificationCode3Track1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CardholderVerificationCode3Track1 obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator ushort(CardholderVerificationCode3Track1 value) => value._Value;

    #endregion
}