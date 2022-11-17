using System;

namespace Play.Globalization.Currency;

/// <summary>
///     Identifies a currency according to ISO 4217
/// </summary>
public record Currency
{
    #region Instance Values

    private readonly Alpha3CurrencyCode _Alpha3;
    private readonly NumericCurrencyCode _Numeric;
    private readonly string _CurrencySymbol;
    private readonly int _Precision;

    #endregion

    #region Constructor

    internal Currency(NumericCurrencyCode numericCode, Alpha3CurrencyCode alpha3Code, string currencySymbol, int precision)
    {
        _Numeric = numericCode;
        _Alpha3 = alpha3Code;
        _CurrencySymbol = currencySymbol;
        _Precision = precision;
    }

    #endregion

    #region Instance Members

    public Alpha3CurrencyCode GetAlpha3Code() => _Alpha3;
    public NumericCurrencyCode GetNumericCode() => _Numeric;
    public int GetMinorUnitLength() => _Precision;
    public string GetCurrencySymbol() => _CurrencySymbol;
    public decimal ToLocalDecimalAmount(int amount) => (decimal) (amount / Math.Pow(10, _Precision));

    #endregion

    #region Equality

    public bool Equals(Currency? x, Currency? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    #endregion
}