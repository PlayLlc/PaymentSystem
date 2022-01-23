using System;

using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Exceptions;

namespace Play.Globalization.Language;

/// <summary>
///     ISO 639-1 compliant identifiers relating language
/// </summary>
public readonly struct Alpha2LanguageCode
{
    #region Static Metadata

    private static readonly AlphaNumeric _AlphaNumericCodec = PlayEncoding.AlphaNumeric;

    #endregion

    #region Instance Values

    // The alpha 2 language identifiers will all be simple ascii so a byte for each char is fine here
    private readonly byte _FirstChar;
    private readonly byte _SecondChar;

    #endregion

    #region Constructor

    public Alpha2LanguageCode(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));
        CheckCore.ForExactLength(value, 2, nameof(value));

        if (!_AlphaNumericCodec.IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was expecting a decimal representation of an ASCII alphabetic character");
        }

        _FirstChar = value[0];
        _SecondChar = value[1];
    }

    public Alpha2LanguageCode(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));
        CheckCore.ForExactLength(value, 2, nameof(value));

        if (!_AlphaNumericCodec.IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was expecting a decimal representation of an ASCII alphabetic character");
        }

        _FirstChar = (byte) value[0];
        _SecondChar = (byte) value[1];
    }

    public Alpha2LanguageCode(byte firstChar, byte secondChar)
    {
        if (!_AlphaNumericCodec.IsValid(firstChar))
        {
            throw new ArgumentOutOfRangeException(nameof(firstChar),
                $"The argument {firstChar} was out of range of an alphabetic Ascii value");
        }

        if (!_AlphaNumericCodec.IsValid(secondChar))
        {
            throw new ArgumentOutOfRangeException(nameof(firstChar),
                $"The argument {firstChar} was out of range of an alphabetic Ascii value");
        }

        _FirstChar = firstChar;
        _SecondChar = secondChar;
    }

    #endregion

    #region Instance Members

    public char[] AsCharArray() => new[] {(char) _FirstChar, (char) _SecondChar};
    public ReadOnlySpan<char> AsReadOnlySpan() => AsCharArray();
    public string AsString() => new(AsReadOnlySpan());
    public override string ToString() => AsString();

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is Alpha2LanguageCode languageCode && Equals(languageCode);
    public bool Equals(Alpha2LanguageCode other) => (_FirstChar == other._FirstChar) && (_SecondChar == other._SecondChar);
    public bool Equals(Alpha2LanguageCode x, Alpha2LanguageCode y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 470621;

        return unchecked((hash * _FirstChar.GetHashCode()) + (hash * _SecondChar.GetHashCode()));
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Alpha2LanguageCode left, Alpha2LanguageCode right) =>
        (left._FirstChar == right._FirstChar) && (left._SecondChar == right._SecondChar);

    public static explicit operator ushort(Alpha2LanguageCode value) => (ushort) ((value._FirstChar << 8) | value._SecondChar);
    public static implicit operator char[](Alpha2LanguageCode value) => value.AsCharArray();
    public static implicit operator ReadOnlySpan<char>(Alpha2LanguageCode value) => value.AsReadOnlySpan();
    public static bool operator !=(Alpha2LanguageCode left, Alpha2LanguageCode right) => !(left == right);

    #endregion
}

// <summary>