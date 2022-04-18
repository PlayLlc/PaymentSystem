using System;

using Play.Codecs;
using Play.Core.Exceptions;
using Play.Globalization.Language;

namespace Play.Globalization.Currency;

public readonly record struct Alpha3CurrencyCode
{
    #region Instance Values

    // These identifiers will be simple ascii characters so a byte is fine here
    private readonly byte _FirstChar;
    private readonly byte _SecondChar;
    private readonly byte _ThirdChar;

    #endregion

    #region Constructor

    /// <exception cref="PlayInternalException"></exception>
    public Alpha3CurrencyCode(ReadOnlySpan<char> value)
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

    public char[] AsCharArray()
    {
        return new[] {(char) _FirstChar, (char) _SecondChar, (char) _ThirdChar};
    }

    public ReadOnlySpan<char> AsReadOnlySpan() => AsCharArray();
    public string AsString() => new(AsReadOnlySpan());

    #endregion

    #region Equality

    public bool Equals(ReadOnlySpan<char> other)
    {
        if (other.Length != 3)
            return false;

        return (_FirstChar == (byte) other[0]) && (_SecondChar == (byte) other[1]) && (_ThirdChar == (byte) other[2]);
    }

    public int CompareTo(Alpha3CurrencyCode other)
    {
        if (other._FirstChar != _FirstChar)
            return _FirstChar.CompareTo(other._FirstChar);

        if (other._SecondChar != _SecondChar)
            return _SecondChar.CompareTo(other._SecondChar);

        return _ThirdChar.CompareTo(other._ThirdChar);
    }

    #endregion
}