using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

public record CvmList : DataElement<byte[]>
{
    #region Static Metadata

    /// <value>Hex: 5F20 Decimal: 95-32</value>
    public static readonly Tag Tag = 0x8E;

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private static readonly byte _MinByteLength = 10;
    private static readonly byte _MaxByteLength = 250;

    #endregion

    #region Constructor

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="EmvEncodingException"></exception>
    public CvmList(ReadOnlySpan<byte> value) : base(value.ToArray())
    {
        if (value.Length != 2)
            throw new EmvEncodingException($"The length of the {nameof(CvmList)} must be even but the length was {value.Length}");

        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);
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

    public static CvmList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CvmList Decode(ReadOnlySpan<byte> value) => new(value);

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