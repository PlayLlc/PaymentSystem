using System;

namespace Play.Globalization.Currency;

public readonly struct Alpha3CurrencyCode
{
    #region Instance Values

    // These identifiers will be simple ascii characters so a byte is fine here
    private readonly byte _FirstChar;
    private readonly byte _SecondChar;
    private readonly byte _ThirdChar;

    #endregion

    #region Constructor

    public Alpha3CurrencyCode(ReadOnlySpan<char> value)
    {
        // HACK: This validation causes circular references. Let's try and create an EnumObject or something similar that allows some validation logic
        //if (!CurrencyCodeRepository.IsValid(value))
        //{
        //    throw new ArgumentOutOfRangeException(nameof(value),
        //        $"The argument {nameof(value)} must be 3 digits or less according to ISO 4217");
        //}

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

    public override bool Equals(object? obj) => obj is Alpha3CurrencyCode countryCodeAlpha2 && Equals(countryCodeAlpha2);
    public bool Equals(Alpha3CurrencyCode other) => (_FirstChar == other._FirstChar) && (_SecondChar == other._SecondChar) && (_ThirdChar == other._ThirdChar);

    public bool Equals(ReadOnlySpan<char> other)
    {
        if (other.Length != 3)
            return false;

        return (_FirstChar == (byte) other[0]) && (_SecondChar == (byte) other[1]) && (_ThirdChar == (byte) other[2]);
    }

    public bool Equals(Alpha3CurrencyCode x, Alpha3CurrencyCode y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 857;

        return unchecked((hash * _FirstChar.GetHashCode()) + (hash * _SecondChar.GetHashCode())) + (hash * _ThirdChar.GetHashCode());
    }

    #endregion
}