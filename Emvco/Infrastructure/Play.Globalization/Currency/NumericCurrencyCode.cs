namespace Play.Globalization.Currency;

public readonly struct NumericCurrencyCode
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public NumericCurrencyCode(ushort value)
    {
        // TODO: Let's get some kind of validation that doesn't cause circular references

        //if (!CurrencyCodeRepository.IsValid(value))
        //{
        //    throw new ArgumentOutOfRangeException(nameof(value), $"The argument {nameof(value)} must be 3 digits or less according to ISO 4217");
        //}

        _Value = value;
    }

    #endregion

    #region Equality

    public bool Equals(NumericCurrencyCode other) => _Value == other._Value;
    public bool Equals(NumericCurrencyCode x, NumericCurrencyCode y) => x.Equals(y);
    public override bool Equals(object? obj) => obj is NumericCurrencyCode Currency && Equals(Currency);
    public int GetHashCode(NumericCurrencyCode obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 832399;

        unchecked
        {
            int result = 0;
            result += hash * _Value.GetHashCode();

            return result;
        }
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(NumericCurrencyCode left, NumericCurrencyCode right) => left.Equals(right);
    public static explicit operator ushort(NumericCurrencyCode value) => value._Value;
    public static bool operator !=(NumericCurrencyCode left, NumericCurrencyCode right) => !left.Equals(right);

    #endregion
}