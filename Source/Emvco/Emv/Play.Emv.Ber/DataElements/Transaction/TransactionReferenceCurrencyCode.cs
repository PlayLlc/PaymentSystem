using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Code defining the common currency used by the terminal in case the Transaction Currency Code is different from the
///     Application Currency Code
/// </summary>
public record TransactionReferenceCurrencyCode : DataElement<NumericCurrencyCode>, IEqualityComparer<TransactionReferenceCurrencyCode>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F3C;
    private const byte _ByteLength = 2;
    private const byte _CharLength = 3;

    #endregion

    #region Constructor

    public TransactionReferenceCurrencyCode(NumericCurrencyCode value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TransactionReferenceCurrencyCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TransactionReferenceCurrencyCode Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TransactionReferenceCurrencyCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.NumericCodec.DecodeToUInt16(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new TransactionReferenceCurrencyCode(new NumericCurrencyCode(result));
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(TransactionReferenceCurrencyCode? x, TransactionReferenceCurrencyCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionReferenceCurrencyCode obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator NumericCurrencyCode(TransactionReferenceCurrencyCode value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}