using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;

namespace Play.Globalization;

/// <summary>
///     Culture specific profile identifying currency, language, and region
/// </summary>
public class CultureProfile : IEquatable<CultureProfile>, IEqualityComparer<CultureProfile>
{
    #region Instance Values

    private readonly Alpha2CountryCode _Alpha2CountryCode;
    private readonly Alpha2LanguageCode _Alpha2LanguageCode;
    private readonly CultureInfo _CultureInfo;
    private readonly RegionInfo _RegionInfo;

    #endregion

    #region Constructor

    public CultureProfile(NumericCountryCode numericCountryCode, Alpha2LanguageCode alpha2LanguageCode)
    {
        Alpha2CountryCode alpha2CountryCode = CountryCodeRepository.Get(numericCountryCode).GetAlpha2Code();
        _CultureInfo = new CultureInfo(GetMicrosoftCultureCode(alpha2LanguageCode, alpha2CountryCode));
        _RegionInfo = new RegionInfo(_CultureInfo.LCID);
        _Alpha2CountryCode = alpha2CountryCode;
        _Alpha2LanguageCode = alpha2LanguageCode;
    }

    public CultureProfile(Alpha2CountryCode alpha2CountryCode, Alpha2LanguageCode alpha2LanguageCode)
    {
        _CultureInfo = new CultureInfo(GetMicrosoftCultureCode(alpha2LanguageCode, alpha2CountryCode));
        _RegionInfo = new RegionInfo(_CultureInfo.LCID);
        _Alpha2CountryCode = alpha2CountryCode;
        _Alpha2LanguageCode = alpha2LanguageCode;
    }

    #endregion

    #region Instance Members

    public Alpha2CountryCode GetAlpha2CountryCode() => _Alpha2CountryCode;
    public Alpha2LanguageCode GetAlpha2LanguageCode() => _Alpha2LanguageCode;
    public Alpha3CurrencyCode GetAlpha3CurrencyCode() => new(_RegionInfo.ISOCurrencySymbol);

    public string GetFiatFormat(Money amount)
    {
        int minorUnitLength = GetMinorUnitLength();
        decimal toBase = ToBaseFiat((ulong) amount, minorUnitLength);

        return toBase.ToString($"C{minorUnitLength}", _CultureInfo);
    }

    internal static string GetMicrosoftCultureCode(Alpha2LanguageCode languageCode, Alpha2CountryCode countryCode) => $"{languageCode}-{countryCode}";
    public int GetMinorUnitLength() => _CultureInfo.NumberFormat.CurrencyDecimalDigits;
    public NumericCountryCode GetNumericCountryCode() => CountryCodeRepository.Get(_Alpha2CountryCode).GetNumericCode();
    public NumericCurrencyCode GetNumericCurrencyCode() => CurrencyCodeRepository.Get(new Alpha3CurrencyCode(_RegionInfo.ISOCurrencySymbol)).GetNumericCode();
    private decimal ToBaseFiat(ulong amount, int minorUnitLength) => ((decimal) amount / minorUnitLength) <= 0 ? 1 : (int) Math.Pow(minorUnitLength, 10);

    #endregion

    #region Equality

    public bool Equals([AllowNull] CultureProfile other)
    {
        if (other is null)
            return false;

        return (_Alpha2LanguageCode == other._Alpha2LanguageCode) && (_Alpha2CountryCode == other._Alpha2CountryCode);
    }

    public bool Equals(CultureProfile? x, CultureProfile? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals([AllowNull] object obj) => obj is CultureProfile cultureProfile && Equals(cultureProfile);
    public int GetHashCode(CultureProfile obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 3041;

        unchecked
        {
            return (_Alpha2CountryCode.GetHashCode() * hash) + (_Alpha2LanguageCode.GetHashCode() * hash);
        }
    }

    #endregion
}