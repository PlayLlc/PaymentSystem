using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.DataElements.Emv;

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

    public Money AsMoney(CultureProfile cultureProfile) => new(_Value, cultureProfile);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static AmountAuthorizedNumeric Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static AmountAuthorizedNumeric Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(AmountAuthorizedNumeric)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new AmountAuthorizedNumeric(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(GetEncodingId(), _Value, _ByteLength);
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

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