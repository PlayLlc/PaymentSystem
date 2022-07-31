using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: Lists a number of card features beyond regular payment.
/// </summary>
public record ApplicationExpirationDate : DataElement<DateTimeUtc>, IEqualityComparer<ApplicationExpirationDate>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x5F24;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 3;
    private const byte _CharLength = 6;

    #endregion

    #region Constructor

    public ApplicationExpirationDate(DateTimeUtc value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationExpirationDate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ApplicationExpirationDate Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationExpirationDate Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.NumericCodec.DecodeToUInt32(value);

        nint charLength = result.GetNumberOfDigits();

        Check.Primitive.ForCharLength(charLength, _CharLength, Tag);

        return new ApplicationExpirationDate(new DateTimeUtc(value[0], value[1], value[2]));
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value.EncodeDate(), _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value.EncodeDate(), length);

    #endregion

    #region Equality

    public bool Equals(ApplicationExpirationDate? x, ApplicationExpirationDate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationExpirationDate obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator DateTimeUtc(ApplicationExpirationDate value) => value._Value;

    #endregion
}