using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Language;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     1-4 languages stored in order of preference, each represented by 2 alphabetical characters according to ISO 639
///     Note: EMVCo strongly recommends that cards be personalized with data element '5F2D' coded in lowercase, but that
///     terminals accept the data element whether it is coded in upper or lower case.
/// </summary>
public record LanguagePreference : DataElement<Alpha2LanguageCode[]>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    public static readonly Tag Tag = 0x5F2D;

    #endregion

    #region Constructor

    public LanguagePreference(ulong value) : base(GetAlpha2LanguageCodes(value))
    { }

    public LanguagePreference(params Alpha2LanguageCode[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    private static Alpha2LanguageCode[] GetAlpha2LanguageCodes(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));
        CheckCore.ForMaximumLength(value, 8, nameof(value));

        if ((value.Length % 2) != 0)
        {
            throw new ArgumentOutOfRangeException(
                $"The argument {nameof(value)} provided was out of range. {nameof(Alpha2LanguageCode)} values are comprised of two characters");
        }

        Span<Alpha2LanguageCode> buffer = stackalloc Alpha2LanguageCode[value.Length / 2];

        for (int i = 0; i < value.Length; i++)
            buffer[i] = new Alpha2LanguageCode(value[i], value[++i]);

        return buffer.ToArray();
    }

    private static Alpha2LanguageCode[] GetAlpha2LanguageCodes(ulong value)
    {
        ulong temp = value;

        Alpha2LanguageCode[] result = (value.GetNumberOfDigits() % 3) == 0
            ? new Alpha2LanguageCode[value.GetNumberOfDigits() / 3]
            : new Alpha2LanguageCode[(value.GetNumberOfDigits() / 3) + 1];

        for (int i = 0; i < result.Length; i++)
            result[i] = new Alpha2LanguageCode((byte) (temp >> Specs.Integer.UInt8.BitCount), (byte) temp);

        return result;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ushort GetByteCount() => checked((ushort) (_Value.Length * 2));
    public Alpha2LanguageCode[] GetLanguageCodes() => _Value;
    public Alpha2LanguageCode GetPreferredLanguage() => _Value[0];
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public static bool StaticEquals(LanguagePreference? x, LanguagePreference? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    public static LanguagePreference Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override LanguagePreference Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static LanguagePreference Decode(ReadOnlySpan<byte> value)
    {
        const ushort minByteLength = 2;
        const ushort maxByteLength = 8;

        if (value.Length is < minByteLength and <= maxByteLength)
        {
            throw new DataElementParsingException(
                $"The Primitive Value {nameof(LanguagePreference)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        return new LanguagePreference(GetAlpha2LanguageCodes(value));
    }

    public override byte[] EncodeValue()
    {
        Span<byte> buffer = stackalloc byte[_Value.Length];

        for (int i = 0, j = 0; i < _Value.Length;)
        {
            ushort tempLanguage = (ushort) _Value[i++];
            buffer[j++] = (byte) (tempLanguage >> 8);
            buffer[j++] = (byte) tempLanguage;
        }

        return buffer.ToArray();
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ulong(LanguagePreference value)
    {
        if (value._Value.Length == 0)
            return 0;

        ulong result = 0;

        for (int i = 0; i < value._Value.Length; i++)
        {
            result <<= Specs.Integer.UInt16.BitCount;
            result |= (ushort) value._Value[i];
        }

        return result;
    }

    #endregion
}