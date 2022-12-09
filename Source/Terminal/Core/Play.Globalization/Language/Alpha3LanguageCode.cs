using System;

using Play.Codecs;
using Play.Core.Exceptions;

namespace Play.Globalization.Language;

/// <summary>
///     ISO 639-2 compliant identifiers relating language
/// </summary>
public readonly struct Alpha3LanguageCode
{
    #region Instance Values

    // The alpha 2 language identifiers will all be simple ascii so a byte for each char is fine here
    private readonly byte _FirstChar;
    private readonly byte _SecondChar;
    private readonly byte _ThirdChar;

    #endregion

    #region Constructor

    /// <exception cref="PlayInternalException"></exception>
    public Alpha3LanguageCode(ReadOnlySpan<byte> value)
    {
        CheckCore.ForExactLength(value, 3, nameof(value));

        if (!PlayCodec.AlphabeticCodec.IsValid(value))
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was expecting a decimal representation of an AsciiCodec alphabetic character"));
        }

        _FirstChar = value[0];
        _SecondChar = value[1];
        _ThirdChar = value[2];
    }

    /// <exception cref="PlayInternalException"></exception>
    public Alpha3LanguageCode(ReadOnlySpan<char> value)
    {
        CheckCore.ForExactLength(value, 3, nameof(value));

        if (!PlayCodec.AlphabeticCodec.IsValid(value))
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was expecting a decimal representation of an AsciiCodec alphabetic character"));
        }

        _FirstChar = (byte) value[0];
        _SecondChar = (byte) value[1];
        _ThirdChar = (byte) value[2];
    }

    #endregion

    #region Instance Members

    public char[] AsCharArray() => new[] {(char) _FirstChar, (char) _SecondChar, (char) _ThirdChar};
    public ReadOnlySpan<char> AsReadOnlySpan() => AsCharArray();
    public string AsString() => new(AsReadOnlySpan());
    public override string ToString() => AsString();

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is Alpha3LanguageCode languageCode && Equals(languageCode);
    public bool Equals(Alpha3LanguageCode other) => (_FirstChar == other._FirstChar) && (_SecondChar == other._SecondChar) && (_ThirdChar == other._ThirdChar);
    public bool Equals(Alpha3LanguageCode x, Alpha3LanguageCode y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 470621;

        return unchecked((hash * _FirstChar.GetHashCode()) + (hash * _SecondChar.GetHashCode()) + (hash * _ThirdChar.GetHashCode()));
    }

    public int CompareTo(Alpha3LanguageCode other)
    {
        if (other._FirstChar != _FirstChar)
            return _FirstChar.CompareTo(other._FirstChar);

        if (other._SecondChar != _SecondChar)
            return _SecondChar.CompareTo(other._SecondChar);

        return _ThirdChar.CompareTo(other._ThirdChar);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Alpha3LanguageCode left, Alpha3LanguageCode right) =>
        (left._FirstChar == right._FirstChar) && (left._SecondChar == right._SecondChar);

    public static explicit operator ushort(Alpha3LanguageCode value) => (ushort) (value._FirstChar | value._SecondChar);
    public static implicit operator char[](Alpha3LanguageCode value) => value.AsCharArray();
    public static implicit operator ReadOnlySpan<char>(Alpha3LanguageCode value) => value.AsReadOnlySpan();
    public static bool operator !=(Alpha3LanguageCode left, Alpha3LanguageCode right) => !(left == right);

    #endregion
}