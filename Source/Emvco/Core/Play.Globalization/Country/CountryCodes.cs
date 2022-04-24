using System;
using System.Collections.Generic;

namespace Play.Globalization.Country;

/// <summary>
///     ISO 3166 compliant identifiers relating to a country or region
/// </summary>
public class CountryCodes : IEqualityComparer<CountryCodes>, IEquatable<CountryCodes>
{
    #region Instance Values

    private readonly Alpha2CountryCode _Alpha2;
    private readonly NumericCountryCode _Numeric;

    #endregion

    #region Constructor

    public CountryCodes(NumericCountryCode numeric, Alpha2CountryCode alpha2)
    {
        _Numeric = numeric;
        _Alpha2 = alpha2;
    }

    #endregion

    #region Instance Members

    public Alpha2CountryCode GetAlpha2Code() => _Alpha2;
    public NumericCountryCode GetNumericCode() => _Numeric;

    #endregion

    #region Equality

    public bool Equals(CountryCodes? x, CountryCodes? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(CountryCodes? other)
    {
        if (other == null)
            return false;

        return (_Numeric == other._Numeric) && (_Alpha2 == other._Alpha2);
    }

    public override bool Equals(object other) => other is CountryCodes languageCodes && Equals(languageCodes);
    public int GetHashCode(CountryCodes obj) => obj.GetHashCode();
    public override int GetHashCode() => _Numeric.GetHashCode() + _Alpha2.GetHashCode();

    #endregion
}