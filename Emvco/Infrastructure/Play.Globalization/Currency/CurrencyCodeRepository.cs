using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Play.Globalization.Currency;

// HACK: That's a lot of memory. Let's make sure we're using values from the database and not storing these values in code. A Terminal will pull the globalization configuration directly from the database so this is only for testing and stuff
public static class CurrencyCodeRepository
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<Alpha3CurrencyCode, Currency> _Alpha3CurrencyMap;
    private static readonly char[] _Buffer = new char[3];
    private static readonly ImmutableSortedDictionary<NumericCurrencyCode, Currency> _NumericCurrencyMap;

    #endregion

    #region Constructor

    static CurrencyCodeRepository()
    {
        List<Currency> mapper = CreateCurrencyCodes(GetFormatMap());

        _NumericCurrencyMap = mapper.ToImmutableSortedDictionary(a => a.GetNumericCode(), b => b);
        _Alpha3CurrencyMap = mapper.ToImmutableSortedDictionary(a => a.GetAlpha3Code(), b => b);
    }

    #endregion

    #region Instance Members

    public static Currency Get(NumericCurrencyCode numericCode) => _NumericCurrencyMap[numericCode];
    public static Currency Get(Alpha3CurrencyCode alphaCode) => _Alpha3CurrencyMap[alphaCode];

    private static Dictionary<string, NumberFormatInfo> GetFormatMap()
    {
        return CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => !x.Equals(CultureInfo.InvariantCulture)).Where(x => !x.IsNeutralCulture)
            .DistinctBy(a => new RegionInfo(a.LCID).ISOCurrencySymbol).ToDictionary(a => new RegionInfo(a.LCID).ISOCurrencySymbol, b => b.NumberFormat);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public static Currency ResolveCurrency(ushort numericCurrencyCode, string alpha3CurrencyCode, Dictionary<string, NumberFormatInfo> formatMap)
    {
        // We're throwing here because there's adding and subtracting we have to do when processing transactions
        // so we need to make sure we only include currencies when we're able to determine their precision
        if (!formatMap.ContainsKey(alpha3CurrencyCode))
            throw new InvalidOperationException();

        return new Currency(new NumericCurrencyCode(numericCurrencyCode), new Alpha3CurrencyCode(alpha3CurrencyCode),
            formatMap[alpha3CurrencyCode].CurrencySymbol, formatMap[alpha3CurrencyCode].CurrencyDecimalDigits);
    }

    private static List<Currency> CreateCurrencyCodes(Dictionary<string, NumberFormatInfo> formatMap) =>
        new()
        {
            ResolveCurrency(971, "AFN", formatMap),
            ResolveCurrency(008, "ALL", formatMap),
            ResolveCurrency(012, "DZD", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(973, "AOA", formatMap),
            ResolveCurrency(951, "XCD", formatMap),
            ResolveCurrency(951, "XCD", formatMap),
            ResolveCurrency(032, "ARS", formatMap),
            ResolveCurrency(051, "AMD", formatMap),
            ResolveCurrency(533, "AWG", formatMap),
            ResolveCurrency(036, "AUD", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(944, "AZN", formatMap),
            ResolveCurrency(044, "BSD", formatMap),
            ResolveCurrency(048, "BHD", formatMap),
            ResolveCurrency(050, "BDT", formatMap),
            ResolveCurrency(052, "BBD", formatMap),
            ResolveCurrency(933, "BYN", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(084, "BZD", formatMap),
            ResolveCurrency(952, "XOF", formatMap),
            ResolveCurrency(060, "BMD", formatMap),
            ResolveCurrency(064, "BTN", formatMap),
            ResolveCurrency(356, "INR", formatMap),
            ResolveCurrency(068, "BOB", formatMap),
            ResolveCurrency(984, "BOV", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(977, "BAM", formatMap),
            ResolveCurrency(072, "BWP", formatMap),
            ResolveCurrency(578, "NOK", formatMap),
            ResolveCurrency(986, "BRL", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(096, "BND", formatMap),
            ResolveCurrency(975, "BGN", formatMap),
            ResolveCurrency(952, "XOF", formatMap),
            ResolveCurrency(108, "BIF", formatMap),
            ResolveCurrency(132, "CVE", formatMap),
            ResolveCurrency(116, "KHR", formatMap),
            ResolveCurrency(950, "XAF", formatMap),
            ResolveCurrency(124, "CAD", formatMap),
            ResolveCurrency(136, "KYD", formatMap),
            ResolveCurrency(950, "XAF", formatMap),
            ResolveCurrency(950, "XAF", formatMap),
            ResolveCurrency(990, "CLF", formatMap),
            ResolveCurrency(152, "CLP", formatMap),
            ResolveCurrency(156, "CNY", formatMap),
            ResolveCurrency(036, "AUD", formatMap),
            ResolveCurrency(036, "AUD", formatMap),
            ResolveCurrency(170, "COP", formatMap),
            ResolveCurrency(970, "COU", formatMap),
            ResolveCurrency(174, "KMF", formatMap),
            ResolveCurrency(976, "CDF", formatMap),
            ResolveCurrency(950, "XAF", formatMap),
            ResolveCurrency(554, "NZD", formatMap),
            ResolveCurrency(188, "CRC", formatMap),
            ResolveCurrency(191, "HRK", formatMap),
            ResolveCurrency(931, "CUC", formatMap),
            ResolveCurrency(192, "CUP", formatMap),
            ResolveCurrency(532, "ANG", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(203, "CZK", formatMap),
            ResolveCurrency(952, "XOF", formatMap),
            ResolveCurrency(208, "DKK", formatMap),
            ResolveCurrency(262, "DJF", formatMap),
            ResolveCurrency(951, "XCD", formatMap),
            ResolveCurrency(214, "DOP", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(818, "EGP", formatMap),
            ResolveCurrency(222, "SVC", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(950, "XAF", formatMap),
            ResolveCurrency(232, "ERN", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(230, "ETB", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(238, "FKP", formatMap),
            ResolveCurrency(208, "DKK", formatMap),
            ResolveCurrency(242, "FJD", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(953, "XPF", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(950, "XAF", formatMap),
            ResolveCurrency(270, "GMD", formatMap),
            ResolveCurrency(981, "GEL", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(936, "GHS", formatMap),
            ResolveCurrency(292, "GIP", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(208, "DKK", formatMap),
            ResolveCurrency(951, "XCD", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(320, "GTQ", formatMap),
            ResolveCurrency(826, "GBP", formatMap),
            ResolveCurrency(324, "GNF", formatMap),
            ResolveCurrency(952, "XOF", formatMap),
            ResolveCurrency(328, "GYD", formatMap),
            ResolveCurrency(332, "HTG", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(036, "AUD", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(340, "HNL", formatMap),
            ResolveCurrency(344, "HKD", formatMap),
            ResolveCurrency(348, "HUF", formatMap),
            ResolveCurrency(352, "ISK", formatMap),
            ResolveCurrency(356, "INR", formatMap),
            ResolveCurrency(360, "IDR", formatMap),
            ResolveCurrency(960, "XDR", formatMap),
            ResolveCurrency(364, "IRR", formatMap),
            ResolveCurrency(368, "IQD", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(826, "GBP", formatMap),
            ResolveCurrency(376, "ILS", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(388, "JMD", formatMap),
            ResolveCurrency(392, "JPY", formatMap),
            ResolveCurrency(826, "GBP", formatMap),
            ResolveCurrency(400, "JOD", formatMap),
            ResolveCurrency(398, "KZT", formatMap),
            ResolveCurrency(404, "KES", formatMap),
            ResolveCurrency(036, "AUD", formatMap),
            ResolveCurrency(408, "KPW", formatMap),
            ResolveCurrency(410, "KRW", formatMap),
            ResolveCurrency(414, "KWD", formatMap),
            ResolveCurrency(417, "KGS", formatMap),
            ResolveCurrency(418, "LAK", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(422, "LBP", formatMap),
            ResolveCurrency(426, "LSL", formatMap),
            ResolveCurrency(710, "ZAR", formatMap),
            ResolveCurrency(430, "LRD", formatMap),
            ResolveCurrency(434, "LYD", formatMap),
            ResolveCurrency(756, "CHF", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(446, "MOP", formatMap),
            ResolveCurrency(969, "MGA", formatMap),
            ResolveCurrency(454, "MWK", formatMap),
            ResolveCurrency(458, "MYR", formatMap),
            ResolveCurrency(462, "MVR", formatMap),
            ResolveCurrency(952, "XOF", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(929, "MRU", formatMap),
            ResolveCurrency(480, "MUR", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(965, "XUA", formatMap),
            ResolveCurrency(484, "MXN", formatMap),
            ResolveCurrency(979, "MXV", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(498, "MDL", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(496, "MNT", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(951, "XCD", formatMap),
            ResolveCurrency(504, "MAD", formatMap),
            ResolveCurrency(943, "MZN", formatMap),
            ResolveCurrency(104, "MMK", formatMap),
            ResolveCurrency(516, "NAD", formatMap),
            ResolveCurrency(710, "ZAR", formatMap),
            ResolveCurrency(036, "AUD", formatMap),
            ResolveCurrency(524, "NPR", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(953, "XPF", formatMap),
            ResolveCurrency(554, "NZD", formatMap),
            ResolveCurrency(558, "NIO", formatMap),
            ResolveCurrency(952, "XOF", formatMap),
            ResolveCurrency(566, "NGN", formatMap),
            ResolveCurrency(554, "NZD", formatMap),
            ResolveCurrency(036, "AUD", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(578, "NOK", formatMap),
            ResolveCurrency(512, "OMR", formatMap),
            ResolveCurrency(586, "PKR", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(590, "PAB", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(598, "PGK", formatMap),
            ResolveCurrency(600, "PYG", formatMap),
            ResolveCurrency(604, "PEN", formatMap),
            ResolveCurrency(608, "PHP", formatMap),
            ResolveCurrency(554, "NZD", formatMap),
            ResolveCurrency(985, "PLN", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(840, "USD", formatMap),
            ResolveCurrency(634, "QAR", formatMap),
            ResolveCurrency(807, "MKD", formatMap),
            ResolveCurrency(946, "RON", formatMap),
            ResolveCurrency(643, "RUB", formatMap),
            ResolveCurrency(646, "RWF", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(654, "SHP", formatMap),
            ResolveCurrency(951, "XCD", formatMap),
            ResolveCurrency(951, "XCD", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(951, "XCD", formatMap),
            ResolveCurrency(882, "WST", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(930, "STN", formatMap),
            ResolveCurrency(682, "SAR", formatMap),
            ResolveCurrency(952, "XOF", formatMap),
            ResolveCurrency(941, "RSD", formatMap),
            ResolveCurrency(690, "SCR", formatMap),
            ResolveCurrency(694, "SLL", formatMap),
            ResolveCurrency(702, "SGD", formatMap),
            ResolveCurrency(532, "ANG", formatMap),
            ResolveCurrency(994, "XSU", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(978, "EUR", formatMap),
            ResolveCurrency(090, "SBD", formatMap),
            ResolveCurrency(706, "SOS", formatMap)
        };

    public static bool IsValid(ushort numericCode)
    {
        return _NumericCurrencyMap.Keys.Any(a => (ushort) a == numericCode);
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static bool IsValid(ReadOnlySpan<char> alpha3Code)
    {
        if (alpha3Code.Length != 3)
            throw new ArgumentOutOfRangeException(nameof(alpha3Code), $"The argument {nameof(alpha3Code)} must be three characters in length");

        _Buffer[0] = alpha3Code[0];
        _Buffer[1] = alpha3Code[1];
        _Buffer[2] = alpha3Code[2];

        return _Alpha3CurrencyMap.Keys.Any(a => a == _Buffer);
    }

    #endregion
}