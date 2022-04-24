using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public record ApplicationEffectiveDate : DataElement<uint>, IEqualityComparer<ApplicationEffectiveDate>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x5F25;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public ApplicationEffectiveDate(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationEffectiveDate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ApplicationEffectiveDate Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationEffectiveDate Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.NumericCodec.DecodeToUInt32(value);

        return new ApplicationEffectiveDate(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(ApplicationEffectiveDate? x, ApplicationEffectiveDate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationEffectiveDate obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator uint(ApplicationEffectiveDate value) => value._Value;

    #endregion
}