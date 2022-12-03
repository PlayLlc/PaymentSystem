using System;

using Play.Codecs;
using Play.Core.Exceptions;

namespace Play.Globalization.Language;

/// <summary>
///     ISO 639-1 compliant identifiers relating language
/// </summary>
public readonly record struct Alpha2LanguageCode
{
    #region Instance Values

    // The alpha 2 language identifiers will all be simple ascii so a byte for each char is fine here
    private readonly byte _FirstChar;
    private readonly byte _SecondChar;

    #endregion

    #region Constructor

    /// <exception cref="PlayInternalException"></exception>
    public Alpha2LanguageCode(ReadOnlySpan<byte> value)
    {
        CheckCore.ForExactLength(value, 2, nameof(value));

        if (!PlayCodec.AlphabeticCodec.IsValid(value))
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was expecting a decimal representation of an AsciiCodec alphabetic character"));
        }

        _FirstChar = value[0];
        _SecondChar = value[1];
    }

    /// <exception cref="PlayInternalException"></exception>
    public Alpha2LanguageCode(ReadOnlySpan<char> value)
    {
        CheckCore.ForExactLength(value, 2, nameof(value));

        if (!PlayCodec.AlphabeticCodec.IsValid(value))
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was expecting a decimal representation of an AsciiCodec alphabetic character"));
        }

        _FirstChar = (byte) value[0];
        _SecondChar = (byte) value[1];
    }

    /// <exception cref="PlayInternalException"></exception>
    public Alpha2LanguageCode(byte firstChar, byte secondChar)
    {
        if (!PlayCodec.AlphabeticCodec.IsValid(firstChar))
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(firstChar),
                $"The argument {firstChar} was out of range of an alphabetic AsciiCodec value"));
        }

        if (!PlayCodec.AlphaNumericCodec.IsValid(secondChar))
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(firstChar),
                $"The argument {firstChar} was out of range of an alphabetic AsciiCodec value"));
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

    public int CompareTo(Alpha2LanguageCode other)
    {
        if (other._FirstChar == _FirstChar)
            return _SecondChar.CompareTo(other._SecondChar);

        return _FirstChar.CompareTo(other._FirstChar);
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ushort(Alpha2LanguageCode value) => (ushort) ((value._FirstChar << 8) | value._SecondChar);

    #endregion
}

// <summary>