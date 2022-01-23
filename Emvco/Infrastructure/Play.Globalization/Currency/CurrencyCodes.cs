using System;
using System.Collections.Generic;

namespace Play.Globalization.Currency;

/// <summary>
///     Identifies a currency according to ISO 4217
/// </summary>
public class CurrencyCodes : IEqualityComparer<CurrencyCodes>, IEquatable<CurrencyCodes>
{
    #region Instance Values

    private readonly Alpha3CurrencyCode _Alpha3;
    private readonly NumericCurrencyCode _Numeric;

    #endregion

    #region Constructor

    internal CurrencyCodes(NumericCurrencyCode numericCode, Alpha3CurrencyCode alpha3Code)
    {
        _Numeric = numericCode;
        _Alpha3 = alpha3Code;
    }

    #endregion

    #region Instance Members

    public Alpha3CurrencyCode GetAlpha3Code() => _Alpha3;
    public NumericCurrencyCode GetNumericCode() => _Numeric;

    #endregion

    #region Equality

    public bool Equals(CurrencyCodes? x, CurrencyCodes? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(CurrencyCodes? other)
    {
        if (other == null)
            return false;

        return (_Alpha3 == other._Alpha3) && (_Numeric == other._Numeric);
    }

    public override bool Equals(object? other) => other is CurrencyCodes currencyCodes && Equals(currencyCodes);
    public int GetHashCode(CurrencyCodes obj) => obj.GetHashCode();
    public override int GetHashCode() => _Alpha3.GetHashCode() + _Numeric.GetHashCode();

    #endregion
}