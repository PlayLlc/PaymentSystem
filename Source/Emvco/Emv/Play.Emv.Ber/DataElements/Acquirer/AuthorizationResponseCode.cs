using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Code that defines the disposition of a message
/// </summary>
public record AuthorizationResponseCode : DataElement<char[]>, IEqualityComparer<AuthorizationResponseCode>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x8A;
    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const byte _ByteLength = 2;
    private const byte _CharLength = 2;

    #endregion

    #region Constructor

    public AuthorizationResponseCode(char[] value) : base(value)
    { }

    #endregion

    #region Serialization

    public static AuthorizationResponseCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override AuthorizationResponseCode Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static AuthorizationResponseCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        char[] result = PlayCodec.AlphaNumericCodec.DecodeToChars(value);

        Check.Primitive.ForMaxCharLength(result.Length, _CharLength, Tag);

        return new AuthorizationResponseCode(result);
    }

    public override byte[] EncodeValue() => PlayCodec.AlphaNumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.AlphaNumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(AuthorizationResponseCode? x, AuthorizationResponseCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AuthorizationResponseCode obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}