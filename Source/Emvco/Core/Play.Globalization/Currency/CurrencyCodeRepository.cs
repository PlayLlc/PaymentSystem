using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

using Play.Core.Exceptions;

namespace Play.Globalization.Currency;

// HACK: That's a lot of memory. Let's make sure we're using values from the database and not storing these values in code. A Terminal will pull the globalization configuration directly from the database so this is only for testing and stuff
public static class CurrencyCodeRepository
{
    #region Static Metadata

    private static readonly ImmutableDictionary<Alpha3CurrencyCode, Currency> _Alpha3CurrencyMap;
    private static readonly char[] _Buffer = new char[3];
    private static readonly ImmutableDictionary<NumericCurrencyCode, Currency> _NumericCurrencyMap;

    #endregion

    #region Constructor

    static CurrencyCodeRepository()
    {
        List<Currency> mapper = CreateCurrencyCodes(GetFormatMap());

        _NumericCurrencyMap = mapper.ToImmutableDictionary(a => a.GetNumericCode(), b => b);
        _Alpha3CurrencyMap = mapper.ToImmutableDictionary(a => a.GetAlpha3Code(), b => b);
    }

    #endregion

    #region Instance Members

    public static List<Currency> GetAll() => _Alpha3CurrencyMap.Values.ToList();
    public static Currency Get(NumericCurrencyCode numericCode) => _NumericCurrencyMap[numericCode];
    public static Currency Get(Alpha3CurrencyCode alphaCode) => _Alpha3CurrencyMap[alphaCode];

    private static Dictionary<string, NumberFormatInfo> GetFormatMap()
    {
        return CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => !x.Equals(CultureInfo.InvariantCulture)).Where(x => !x.IsNeutralCulture)
            .DistinctBy(a => new RegionInfo(a.LCID).ISOCurrencySymbol).ToDictionary(a => new RegionInfo(a.LCID).ISOCurrencySymbol, b => b.NumberFormat);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    public static Currency? ResolveCurrency(ushort numericCurrencyCode, string alpha3CurrencyCode, Dictionary<string, NumberFormatInfo> formatMap)
    {
        // We're throwing here because there's adding and subtracting we have to do when processing transactions
        // so we need to make sure we only include currencies when we're able to determine their precision
        //if (!formatMap.ContainsKey(alpha3CurrencyCode))
        //    return new Currency(new NumericCurrencyCode(numericCurrencyCode), new Alpha3CurrencyCode(alpha3CurrencyCode),
        //        string.Empty, 0);

        if (!formatMap.ContainsKey(alpha3CurrencyCode))
            return null;

        return new Currency(new NumericCurrencyCode(numericCurrencyCode), new Alpha3CurrencyCode(alpha3CurrencyCode),
            formatMap[alpha3CurrencyCode].CurrencySymbol, formatMap[alpha3CurrencyCode].CurrencyDecimalDigits);
    }

    private static List<Currency> CreateCurrencyCodes(Dictionary<string, NumberFormatInfo> formatMap)
    {
        HashSet<Currency?> hash = new HashSet<Currency?>();

        hash.Add(ResolveCurrency(971, "AFN", formatMap));
        hash.Add(ResolveCurrency(008, "ALL", formatMap));
        hash.Add(ResolveCurrency(012, "DZD", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(973, "AOA", formatMap));
        hash.Add(ResolveCurrency(951, "XCD", formatMap));
        hash.Add(ResolveCurrency(951, "XCD", formatMap));
        hash.Add(ResolveCurrency(032, "ARS", formatMap));
        hash.Add(ResolveCurrency(051, "AMD", formatMap));
        hash.Add(ResolveCurrency(533, "AWG", formatMap));
        hash.Add(ResolveCurrency(036, "AUD", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(944, "AZN", formatMap));
        hash.Add(ResolveCurrency(044, "BSD", formatMap));
        hash.Add(ResolveCurrency(048, "BHD", formatMap));
        hash.Add(ResolveCurrency(050, "BDT", formatMap));
        hash.Add(ResolveCurrency(052, "BBD", formatMap));
        hash.Add(ResolveCurrency(933, "BYN", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(084, "BZD", formatMap));
        hash.Add(ResolveCurrency(952, "XOF", formatMap));
        hash.Add(ResolveCurrency(060, "BMD", formatMap));
        hash.Add(ResolveCurrency(064, "BTN", formatMap));
        hash.Add(ResolveCurrency(356, "INR", formatMap));
        hash.Add(ResolveCurrency(068, "BOB", formatMap));
        hash.Add(ResolveCurrency(984, "BOV", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(977, "BAM", formatMap));
        hash.Add(ResolveCurrency(072, "BWP", formatMap));
        hash.Add(ResolveCurrency(578, "NOK", formatMap));
        hash.Add(ResolveCurrency(986, "BRL", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(096, "BND", formatMap));
        hash.Add(ResolveCurrency(975, "BGN", formatMap));
        hash.Add(ResolveCurrency(952, "XOF", formatMap));
        hash.Add(ResolveCurrency(108, "BIF", formatMap));
        hash.Add(ResolveCurrency(132, "CVE", formatMap));
        hash.Add(ResolveCurrency(116, "KHR", formatMap));
        hash.Add(ResolveCurrency(950, "XAF", formatMap));
        hash.Add(ResolveCurrency(124, "CAD", formatMap));
        hash.Add(ResolveCurrency(136, "KYD", formatMap));
        hash.Add(ResolveCurrency(950, "XAF", formatMap));
        hash.Add(ResolveCurrency(950, "XAF", formatMap));
        hash.Add(ResolveCurrency(990, "CLF", formatMap));
        hash.Add(ResolveCurrency(152, "CLP", formatMap));
        hash.Add(ResolveCurrency(156, "CNY", formatMap));
        hash.Add(ResolveCurrency(036, "AUD", formatMap));
        hash.Add(ResolveCurrency(036, "AUD", formatMap));
        hash.Add(ResolveCurrency(170, "COP", formatMap));
        hash.Add(ResolveCurrency(970, "COU", formatMap));
        hash.Add(ResolveCurrency(174, "KMF", formatMap));
        hash.Add(ResolveCurrency(976, "CDF", formatMap));
        hash.Add(ResolveCurrency(950, "XAF", formatMap));
        hash.Add(ResolveCurrency(554, "NZD", formatMap));
        hash.Add(ResolveCurrency(188, "CRC", formatMap));
        hash.Add(ResolveCurrency(191, "HRK", formatMap));
        hash.Add(ResolveCurrency(931, "CUC", formatMap));
        hash.Add(ResolveCurrency(192, "CUP", formatMap));
        hash.Add(ResolveCurrency(532, "ANG", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(203, "CZK", formatMap));
        hash.Add(ResolveCurrency(952, "XOF", formatMap));
        hash.Add(ResolveCurrency(208, "DKK", formatMap));
        hash.Add(ResolveCurrency(262, "DJF", formatMap));
        hash.Add(ResolveCurrency(951, "XCD", formatMap));
        hash.Add(ResolveCurrency(214, "DOP", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(818, "EGP", formatMap));
        hash.Add(ResolveCurrency(222, "SVC", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(950, "XAF", formatMap));
        hash.Add(ResolveCurrency(232, "ERN", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(230, "ETB", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(238, "FKP", formatMap));
        hash.Add(ResolveCurrency(208, "DKK", formatMap));
        hash.Add(ResolveCurrency(242, "FJD", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(953, "XPF", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(950, "XAF", formatMap));
        hash.Add(ResolveCurrency(270, "GMD", formatMap));
        hash.Add(ResolveCurrency(981, "GEL", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(936, "GHS", formatMap));
        hash.Add(ResolveCurrency(292, "GIP", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(208, "DKK", formatMap));
        hash.Add(ResolveCurrency(951, "XCD", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(320, "GTQ", formatMap));
        hash.Add(ResolveCurrency(826, "GBP", formatMap));
        hash.Add(ResolveCurrency(324, "GNF", formatMap));
        hash.Add(ResolveCurrency(952, "XOF", formatMap));
        hash.Add(ResolveCurrency(328, "GYD", formatMap));
        hash.Add(ResolveCurrency(332, "HTG", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(036, "AUD", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(340, "HNL", formatMap));
        hash.Add(ResolveCurrency(344, "HKD", formatMap));
        hash.Add(ResolveCurrency(348, "HUF", formatMap));
        hash.Add(ResolveCurrency(352, "ISK", formatMap));
        hash.Add(ResolveCurrency(356, "INR", formatMap));
        hash.Add(ResolveCurrency(360, "IDR", formatMap));
        hash.Add(ResolveCurrency(960, "XDR", formatMap));
        hash.Add(ResolveCurrency(364, "IRR", formatMap));
        hash.Add(ResolveCurrency(368, "IQD", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(826, "GBP", formatMap));
        hash.Add(ResolveCurrency(376, "ILS", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(388, "JMD", formatMap));
        hash.Add(ResolveCurrency(392, "JPY", formatMap));
        hash.Add(ResolveCurrency(826, "GBP", formatMap));
        hash.Add(ResolveCurrency(400, "JOD", formatMap));
        hash.Add(ResolveCurrency(398, "KZT", formatMap));
        hash.Add(ResolveCurrency(404, "KES", formatMap));
        hash.Add(ResolveCurrency(036, "AUD", formatMap));
        hash.Add(ResolveCurrency(408, "KPW", formatMap));
        hash.Add(ResolveCurrency(410, "KRW", formatMap));
        hash.Add(ResolveCurrency(414, "KWD", formatMap));
        hash.Add(ResolveCurrency(417, "KGS", formatMap));
        hash.Add(ResolveCurrency(418, "LAK", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(422, "LBP", formatMap));
        hash.Add(ResolveCurrency(426, "LSL", formatMap));
        hash.Add(ResolveCurrency(710, "ZAR", formatMap));
        hash.Add(ResolveCurrency(430, "LRD", formatMap));
        hash.Add(ResolveCurrency(434, "LYD", formatMap));
        hash.Add(ResolveCurrency(756, "CHF", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(446, "MOP", formatMap));
        hash.Add(ResolveCurrency(969, "MGA", formatMap));
        hash.Add(ResolveCurrency(454, "MWK", formatMap));
        hash.Add(ResolveCurrency(458, "MYR", formatMap));
        hash.Add(ResolveCurrency(462, "MVR", formatMap));
        hash.Add(ResolveCurrency(952, "XOF", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(929, "MRU", formatMap));
        hash.Add(ResolveCurrency(480, "MUR", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(965, "XUA", formatMap));
        hash.Add(ResolveCurrency(484, "MXN", formatMap));
        hash.Add(ResolveCurrency(979, "MXV", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(498, "MDL", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(496, "MNT", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(951, "XCD", formatMap));
        hash.Add(ResolveCurrency(504, "MAD", formatMap));
        hash.Add(ResolveCurrency(943, "MZN", formatMap));
        hash.Add(ResolveCurrency(104, "MMK", formatMap));
        hash.Add(ResolveCurrency(516, "NAD", formatMap));
        hash.Add(ResolveCurrency(710, "ZAR", formatMap));
        hash.Add(ResolveCurrency(036, "AUD", formatMap));
        hash.Add(ResolveCurrency(524, "NPR", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(953, "XPF", formatMap));
        hash.Add(ResolveCurrency(554, "NZD", formatMap));
        hash.Add(ResolveCurrency(558, "NIO", formatMap));
        hash.Add(ResolveCurrency(952, "XOF", formatMap));
        hash.Add(ResolveCurrency(566, "NGN", formatMap));
        hash.Add(ResolveCurrency(554, "NZD", formatMap));
        hash.Add(ResolveCurrency(036, "AUD", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(578, "NOK", formatMap));
        hash.Add(ResolveCurrency(512, "OMR", formatMap));
        hash.Add(ResolveCurrency(586, "PKR", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(590, "PAB", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(598, "PGK", formatMap));
        hash.Add(ResolveCurrency(600, "PYG", formatMap));
        hash.Add(ResolveCurrency(604, "PEN", formatMap));
        hash.Add(ResolveCurrency(608, "PHP", formatMap));
        hash.Add(ResolveCurrency(554, "NZD", formatMap));
        hash.Add(ResolveCurrency(985, "PLN", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(840, "USD", formatMap));
        hash.Add(ResolveCurrency(634, "QAR", formatMap));
        hash.Add(ResolveCurrency(807, "MKD", formatMap));
        hash.Add(ResolveCurrency(946, "RON", formatMap));
        hash.Add(ResolveCurrency(643, "RUB", formatMap));
        hash.Add(ResolveCurrency(646, "RWF", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(654, "SHP", formatMap));
        hash.Add(ResolveCurrency(951, "XCD", formatMap));
        hash.Add(ResolveCurrency(951, "XCD", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(951, "XCD", formatMap));
        hash.Add(ResolveCurrency(882, "WST", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(930, "STN", formatMap));
        hash.Add(ResolveCurrency(682, "SAR", formatMap));
        hash.Add(ResolveCurrency(952, "XOF", formatMap));
        hash.Add(ResolveCurrency(941, "RSD", formatMap));
        hash.Add(ResolveCurrency(690, "SCR", formatMap));
        hash.Add(ResolveCurrency(694, "SLL", formatMap));
        hash.Add(ResolveCurrency(702, "SGD", formatMap));
        hash.Add(ResolveCurrency(532, "ANG", formatMap));
        hash.Add(ResolveCurrency(994, "XSU", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(978, "EUR", formatMap));
        hash.Add(ResolveCurrency(090, "SBD", formatMap));
        hash.Add(ResolveCurrency(706, "SOS", formatMap));

        hash.RemoveWhere(a => a is null);

        return hash.Cast<Currency>().ToList();
    }

    public static bool IsValid(ushort numericCode)
    {
        return _NumericCurrencyMap.Keys.Any(a => (ushort) a == numericCode);
    }

    #endregion
}