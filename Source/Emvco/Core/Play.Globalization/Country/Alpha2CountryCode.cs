using System;

namespace Play.Globalization.Country;

public readonly record struct Alpha2CountryCode : IComparable<Alpha2CountryCode>
{
    #region Instance Values

    // These identifiers will be simple ascii characters so a byte is fine here
    private readonly byte _FirstChar;
    private readonly byte _SecondChar;

    #endregion

    #region Constructor

    //add extra default parameter to avoid circular dependency.
    public Alpha2CountryCode(ReadOnlySpan<char> value, bool validate = false)
    {
        //Something not working right with the initialization of the repo
        //if (validate && !CountryCodeRepository.IsValid(value))
        //    throw new ArgumentOutOfRangeException(nameof(value), $"The argument {nameof(value)} must be ISO 3166 compliant");

        _FirstChar = (byte) value[0];
        _SecondChar = (byte) value[1];
    }

    #endregion

    #region Equality

    public bool Equals(ReadOnlySpan<char> other)
    {
        if (other.Length != 2)
            throw new ArgumentOutOfRangeException(nameof(other), $"The argument {nameof(other)} must be 2 characters in length according to ISO 3166");

        if (_FirstChar != (byte) other[0])
            return false;

        if (_SecondChar != (byte) other[1])
            return false;

        return true;
    }

    public bool Equals(Alpha2CountryCode other) => (_FirstChar == other._FirstChar) && (_SecondChar == other._SecondChar);
    public bool Equals(Alpha2CountryCode x, Alpha2CountryCode y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 14939;

        return unchecked((hash * _FirstChar.GetHashCode()) + (hash * _SecondChar.GetHashCode()));
    }

    public int CompareTo(Alpha2CountryCode other) => (_FirstChar + _SecondChar).CompareTo(other._FirstChar + other._SecondChar);

    #endregion

    #region Operator Overrides

    public static explicit operator string(Alpha2CountryCode value)
    {
        Span<char> buffer = stackalloc char[2];
        buffer[0] = (char) value._FirstChar;
        buffer[1] = (char) value._SecondChar;

        return new string(buffer);
    }

    #endregion
}