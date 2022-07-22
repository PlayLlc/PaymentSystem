using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

public record CvmList : DataElement<BigInteger>, IResolveXAndYAmountForCvmSelection
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

    /// <summary>
    ///     Checks if there are any CVM Rules present. In addition, <see cref="CvmList" /> with invalid encoding, such as an
    ///     odd number of bytes, is treated as if it is empty
    /// </summary>
    /// <remarks>EMV Book 3 Section 10.5</remarks>
    public bool AreCardholderVerificationRulesPresent() => (_Value.GetByteCount() > 8) && ((_Value.GetByteCount() % 2) == 0);

    /// <exception cref="DataElementParsingException"></exception>
    public bool TryGetCardholderVerificationRules(out CvmRule[]? result)
    {
        if (!AreCardholderVerificationRulesPresent())
        {
            result = Array.Empty<CvmRule>();

            return false;
        }

        const int offset = 8;
        result = new CvmRule[((_Value.GetByteCount() - offset) / 2)];
        Span<byte> valueBuffer = _Value.ToByteArray().AsSpan()[offset..];

        for (int i = 0, j = 0; j < result.Length; j++)
        {
            result[j] = new CvmRule(valueBuffer[i..(i + 2)]);
            i += 2;
        }

        return true;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public Money GetXAmount(NumericCurrencyCode currencyCode) => new(PlayCodec.BinaryCodec.DecodeToUInt64(_Value.ToByteArray().AsSpan()[..4]), currencyCode);
    public Money GetYAmount(NumericCurrencyCode currencyCode) => new(PlayCodec.BinaryCodec.DecodeToUInt64(_Value.ToByteArray().AsSpan()[4..8]), currencyCode);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    public static CvmList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override CvmList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

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