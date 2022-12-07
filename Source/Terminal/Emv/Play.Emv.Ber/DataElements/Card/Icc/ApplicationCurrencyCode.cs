using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the currency in which the account is managed in accordance with [ISO 4217].
/// </summary>
public record ApplicationCurrencyCode : DataElement<NumericCurrencyCode>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F42;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 2;
    private const byte _CharLength = 3;

    #endregion

    #region Constructor

    public ApplicationCurrencyCode(NumericCurrencyCode value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationCurrencyCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ApplicationCurrencyCode Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationCurrencyCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.NumericCodec.DecodeToUInt16(value);

        nint charLength = result.GetNumberOfDigits();

        Check.Primitive.ForCharLength(charLength, _CharLength, Tag);

        return new ApplicationCurrencyCode(new NumericCurrencyCode(result));
    }

    public override byte[] EncodeValue() => _Value.EncodeValue();
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator NumericCurrencyCode(ApplicationCurrencyCode value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}