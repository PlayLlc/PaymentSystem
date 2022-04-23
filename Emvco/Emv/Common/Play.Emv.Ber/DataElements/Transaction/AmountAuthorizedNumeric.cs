using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Authorized amount of the transaction (excluding adjustments)
/// </summary>
public record AmountAuthorizedNumeric : DataElement<ulong>, IEqualityComparer<AmountAuthorizedNumeric>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F02;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public AmountAuthorizedNumeric(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public Money AsMoney(NumericCurrencyCode numericCurrencyCode) => new(_Value, numericCurrencyCode);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static AmountAuthorizedNumeric Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override AmountAuthorizedNumeric Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static AmountAuthorizedNumeric Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        return new AmountAuthorizedNumeric(result);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(AmountAuthorizedNumeric? x, AmountAuthorizedNumeric? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AmountAuthorizedNumeric obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(AmountAuthorizedNumeric left, ulong right) => left._Value == right;
    public static bool operator ==(ulong left, AmountAuthorizedNumeric right) => left == right._Value;
    public static explicit operator BigInteger(AmountAuthorizedNumeric value) => value._Value;
    public static implicit operator ulong(AmountAuthorizedNumeric value) => value._Value;
    public static bool operator !=(AmountAuthorizedNumeric left, ulong right) => !(left == right);
    public static bool operator !=(ulong left, AmountAuthorizedNumeric right) => !(left == right);

    #endregion
}