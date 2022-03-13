using System;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;

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

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public CardholderVerificationRules[] GetCardholderVerificationRules()
    {
        CardholderVerificationRules[] result = new CardholderVerificationRules[(_Value.Length - 2) / 2];
        for (int i = 2, j = 0; i < result.Length; i += 2, j++)
            result[j] = new CardholderVerificationRules(_Value[i..(i + 2)]);

        return result;
    }

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