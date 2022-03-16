using System;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Services.Conditions;
using Play.Globalization.Currency;

namespace Play.Emv.DataElements;

public record CvmList : DataElement<BigInteger>
{
    #region Static Metadata

    /// <value>Hex: 5F20 Decimal: 95-32</value>
    public static readonly Tag Tag = 0x8E;

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private static readonly byte _MinByteLength = 10;
    private static readonly byte _MaxByteLength = 250;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    public CvmList(BigInteger value) : base(value)
    {
        Check.Primitive.ForMinimumLength((byte) value.GetByteCount(), _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength((byte) value.GetByteCount(), _MaxByteLength, Tag);
    }

    #endregion

    #region Instance Members

    /// <exception cref="DataElementParsingException"></exception>
    public CardholderVerificationRule[] GetCardholderVerificationRules()
    {
        const int offset = 8;
        CardholderVerificationRule[] result = new CardholderVerificationRule[((_Value.GetByteCount() - offset) / 2) - 1];
        Span<byte> valueBuffer = _Value.ToByteArray().AsSpan()[offset..];

        for (int i = 2, j = 0; i < result.Length; j++)
            result[j] = new CardholderVerificationRule(valueBuffer[i++..i++]);

        return result;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public Money GetXAmount(ApplicationCurrencyCode currencyCode) =>
        new(PlayCodec.BinaryCodec.DecodeToUInt64(_Value.ToByteArray().AsSpan()[..4]), (NumericCurrencyCode) currencyCode);

    public Money GetYAmount(ApplicationCurrencyCode currencyCode) =>
        new(PlayCodec.BinaryCodec.DecodeToUInt64(_Value.ToByteArray().AsSpan()[4..8]), (NumericCurrencyCode) currencyCode);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    public static CvmList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    public static CvmList Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new CvmList(result);
    }

    #endregion

    #region Equality

    public bool Equals(CardholderName? x, CardholderName? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CardholderName obj) => obj.GetHashCode();

    #endregion
}