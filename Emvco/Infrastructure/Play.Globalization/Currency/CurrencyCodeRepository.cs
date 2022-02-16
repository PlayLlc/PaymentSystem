using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Play.Globalization.Currency;

internal static class CurrencyCodeRepository
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<Alpha3CurrencyCode, CurrencyCodes> _Alpha3CurrencyMap;
    private static readonly char[] _Buffer = new char[3];
    private static readonly ImmutableSortedDictionary<NumericCurrencyCode, CurrencyCodes> _NumericCurrencyMap;

    #endregion

    #region Constructor

    static CurrencyCodeRepository()
    {
        _NumericCurrencyMap = GetCurrencyCodes().ToImmutableSortedDictionary(a => a.GetNumericCode(), b => b);
        _Alpha3CurrencyMap = GetCurrencyCodes().ToImmutableSortedDictionary(a => a.GetAlpha3Code(), b => b);
    }

    #endregion

    #region Instance Members

    public static CurrencyCodes Get(NumericCurrencyCode numericCode) => _NumericCurrencyMap[numericCode];
    public static CurrencyCodes Get(Alpha3CurrencyCode alphaCode) => _Alpha3CurrencyMap[alphaCode];

    private static List<CurrencyCodes> GetCurrencyCodes() =>
        new()
        {
            new CurrencyCodes(new NumericCurrencyCode(971), new Alpha3CurrencyCode("AFN")),
            new CurrencyCodes(new NumericCurrencyCode(008), new Alpha3CurrencyCode("ALL")),
            new CurrencyCodes(new NumericCurrencyCode(012), new Alpha3CurrencyCode("DZD")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(973), new Alpha3CurrencyCode("AOA")),
            new CurrencyCodes(new NumericCurrencyCode(951), new Alpha3CurrencyCode("XCD")),
            new CurrencyCodes(new NumericCurrencyCode(951), new Alpha3CurrencyCode("XCD")),
            new CurrencyCodes(new NumericCurrencyCode(032), new Alpha3CurrencyCode("ARS")),
            new CurrencyCodes(new NumericCurrencyCode(051), new Alpha3CurrencyCode("AMD")),
            new CurrencyCodes(new NumericCurrencyCode(533), new Alpha3CurrencyCode("AWG")),
            new CurrencyCodes(new NumericCurrencyCode(036), new Alpha3CurrencyCode("AUD")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(944), new Alpha3CurrencyCode("AZN")),
            new CurrencyCodes(new NumericCurrencyCode(044), new Alpha3CurrencyCode("BSD")),
            new CurrencyCodes(new NumericCurrencyCode(048), new Alpha3CurrencyCode("BHD")),
            new CurrencyCodes(new NumericCurrencyCode(050), new Alpha3CurrencyCode("BDT")),
            new CurrencyCodes(new NumericCurrencyCode(052), new Alpha3CurrencyCode("BBD")),
            new CurrencyCodes(new NumericCurrencyCode(933), new Alpha3CurrencyCode("BYN")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(084), new Alpha3CurrencyCode("BZD")),
            new CurrencyCodes(new NumericCurrencyCode(952), new Alpha3CurrencyCode("XOF")),
            new CurrencyCodes(new NumericCurrencyCode(060), new Alpha3CurrencyCode("BMD")),
            new CurrencyCodes(new NumericCurrencyCode(064), new Alpha3CurrencyCode("BTN")),
            new CurrencyCodes(new NumericCurrencyCode(356), new Alpha3CurrencyCode("INR")),
            new CurrencyCodes(new NumericCurrencyCode(068), new Alpha3CurrencyCode("BOB")),
            new CurrencyCodes(new NumericCurrencyCode(984), new Alpha3CurrencyCode("BOV")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(977), new Alpha3CurrencyCode("BAM")),
            new CurrencyCodes(new NumericCurrencyCode(072), new Alpha3CurrencyCode("BWP")),
            new CurrencyCodes(new NumericCurrencyCode(578), new Alpha3CurrencyCode("NOK")),
            new CurrencyCodes(new NumericCurrencyCode(986), new Alpha3CurrencyCode("BRL")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(096), new Alpha3CurrencyCode("BND")),
            new CurrencyCodes(new NumericCurrencyCode(975), new Alpha3CurrencyCode("BGN")),
            new CurrencyCodes(new NumericCurrencyCode(952), new Alpha3CurrencyCode("XOF")),
            new CurrencyCodes(new NumericCurrencyCode(108), new Alpha3CurrencyCode("BIF")),
            new CurrencyCodes(new NumericCurrencyCode(132), new Alpha3CurrencyCode("CVE")),
            new CurrencyCodes(new NumericCurrencyCode(116), new Alpha3CurrencyCode("KHR")),
            new CurrencyCodes(new NumericCurrencyCode(950), new Alpha3CurrencyCode("XAF")),
            new CurrencyCodes(new NumericCurrencyCode(124), new Alpha3CurrencyCode("CAD")),
            new CurrencyCodes(new NumericCurrencyCode(136), new Alpha3CurrencyCode("KYD")),
            new CurrencyCodes(new NumericCurrencyCode(950), new Alpha3CurrencyCode("XAF")),
            new CurrencyCodes(new NumericCurrencyCode(950), new Alpha3CurrencyCode("XAF")),
            new CurrencyCodes(new NumericCurrencyCode(990), new Alpha3CurrencyCode("CLF")),
            new CurrencyCodes(new NumericCurrencyCode(152), new Alpha3CurrencyCode("CLP")),
            new CurrencyCodes(new NumericCurrencyCode(156), new Alpha3CurrencyCode("CNY")),
            new CurrencyCodes(new NumericCurrencyCode(036), new Alpha3CurrencyCode("AUD")),
            new CurrencyCodes(new NumericCurrencyCode(036), new Alpha3CurrencyCode("AUD")),
            new CurrencyCodes(new NumericCurrencyCode(170), new Alpha3CurrencyCode("COP")),
            new CurrencyCodes(new NumericCurrencyCode(970), new Alpha3CurrencyCode("COU")),
            new CurrencyCodes(new NumericCurrencyCode(174), new Alpha3CurrencyCode("KMF")),
            new CurrencyCodes(new NumericCurrencyCode(976), new Alpha3CurrencyCode("CDF")),
            new CurrencyCodes(new NumericCurrencyCode(950), new Alpha3CurrencyCode("XAF")),
            new CurrencyCodes(new NumericCurrencyCode(554), new Alpha3CurrencyCode("NZD")),
            new CurrencyCodes(new NumericCurrencyCode(188), new Alpha3CurrencyCode("CRC")),
            new CurrencyCodes(new NumericCurrencyCode(191), new Alpha3CurrencyCode("HRK")),
            new CurrencyCodes(new NumericCurrencyCode(931), new Alpha3CurrencyCode("CUC")),
            new CurrencyCodes(new NumericCurrencyCode(192), new Alpha3CurrencyCode("CUP")),
            new CurrencyCodes(new NumericCurrencyCode(532), new Alpha3CurrencyCode("ANG")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(203), new Alpha3CurrencyCode("CZK")),
            new CurrencyCodes(new NumericCurrencyCode(952), new Alpha3CurrencyCode("XOF")),
            new CurrencyCodes(new NumericCurrencyCode(208), new Alpha3CurrencyCode("DKK")),
            new CurrencyCodes(new NumericCurrencyCode(262), new Alpha3CurrencyCode("DJF")),
            new CurrencyCodes(new NumericCurrencyCode(951), new Alpha3CurrencyCode("XCD")),
            new CurrencyCodes(new NumericCurrencyCode(214), new Alpha3CurrencyCode("DOP")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(818), new Alpha3CurrencyCode("EGP")),
            new CurrencyCodes(new NumericCurrencyCode(222), new Alpha3CurrencyCode("SVC")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(950), new Alpha3CurrencyCode("XAF")),
            new CurrencyCodes(new NumericCurrencyCode(232), new Alpha3CurrencyCode("ERN")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(230), new Alpha3CurrencyCode("ETB")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(238), new Alpha3CurrencyCode("FKP")),
            new CurrencyCodes(new NumericCurrencyCode(208), new Alpha3CurrencyCode("DKK")),
            new CurrencyCodes(new NumericCurrencyCode(242), new Alpha3CurrencyCode("FJD")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(953), new Alpha3CurrencyCode("XPF")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(950), new Alpha3CurrencyCode("XAF")),
            new CurrencyCodes(new NumericCurrencyCode(270), new Alpha3CurrencyCode("GMD")),
            new CurrencyCodes(new NumericCurrencyCode(981), new Alpha3CurrencyCode("GEL")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(936), new Alpha3CurrencyCode("GHS")),
            new CurrencyCodes(new NumericCurrencyCode(292), new Alpha3CurrencyCode("GIP")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(208), new Alpha3CurrencyCode("DKK")),
            new CurrencyCodes(new NumericCurrencyCode(951), new Alpha3CurrencyCode("XCD")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(320), new Alpha3CurrencyCode("GTQ")),
            new CurrencyCodes(new NumericCurrencyCode(826), new Alpha3CurrencyCode("GBP")),
            new CurrencyCodes(new NumericCurrencyCode(324), new Alpha3CurrencyCode("GNF")),
            new CurrencyCodes(new NumericCurrencyCode(952), new Alpha3CurrencyCode("XOF")),
            new CurrencyCodes(new NumericCurrencyCode(328), new Alpha3CurrencyCode("GYD")),
            new CurrencyCodes(new NumericCurrencyCode(332), new Alpha3CurrencyCode("HTG")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(036), new Alpha3CurrencyCode("AUD")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(340), new Alpha3CurrencyCode("HNL")),
            new CurrencyCodes(new NumericCurrencyCode(344), new Alpha3CurrencyCode("HKD")),
            new CurrencyCodes(new NumericCurrencyCode(348), new Alpha3CurrencyCode("HUF")),
            new CurrencyCodes(new NumericCurrencyCode(352), new Alpha3CurrencyCode("ISK")),
            new CurrencyCodes(new NumericCurrencyCode(356), new Alpha3CurrencyCode("INR")),
            new CurrencyCodes(new NumericCurrencyCode(360), new Alpha3CurrencyCode("IDR")),
            new CurrencyCodes(new NumericCurrencyCode(960), new Alpha3CurrencyCode("XDR")),
            new CurrencyCodes(new NumericCurrencyCode(364), new Alpha3CurrencyCode("IRR")),
            new CurrencyCodes(new NumericCurrencyCode(368), new Alpha3CurrencyCode("IQD")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(826), new Alpha3CurrencyCode("GBP")),
            new CurrencyCodes(new NumericCurrencyCode(376), new Alpha3CurrencyCode("ILS")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(388), new Alpha3CurrencyCode("JMD")),
            new CurrencyCodes(new NumericCurrencyCode(392), new Alpha3CurrencyCode("JPY")),
            new CurrencyCodes(new NumericCurrencyCode(826), new Alpha3CurrencyCode("GBP")),
            new CurrencyCodes(new NumericCurrencyCode(400), new Alpha3CurrencyCode("JOD")),
            new CurrencyCodes(new NumericCurrencyCode(398), new Alpha3CurrencyCode("KZT")),
            new CurrencyCodes(new NumericCurrencyCode(404), new Alpha3CurrencyCode("KES")),
            new CurrencyCodes(new NumericCurrencyCode(036), new Alpha3CurrencyCode("AUD")),
            new CurrencyCodes(new NumericCurrencyCode(408), new Alpha3CurrencyCode("KPW")),
            new CurrencyCodes(new NumericCurrencyCode(410), new Alpha3CurrencyCode("KRW")),
            new CurrencyCodes(new NumericCurrencyCode(414), new Alpha3CurrencyCode("KWD")),
            new CurrencyCodes(new NumericCurrencyCode(417), new Alpha3CurrencyCode("KGS")),
            new CurrencyCodes(new NumericCurrencyCode(418), new Alpha3CurrencyCode("LAK")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(422), new Alpha3CurrencyCode("LBP")),
            new CurrencyCodes(new NumericCurrencyCode(426), new Alpha3CurrencyCode("LSL")),
            new CurrencyCodes(new NumericCurrencyCode(710), new Alpha3CurrencyCode("ZAR")),
            new CurrencyCodes(new NumericCurrencyCode(430), new Alpha3CurrencyCode("LRD")),
            new CurrencyCodes(new NumericCurrencyCode(434), new Alpha3CurrencyCode("LYD")),
            new CurrencyCodes(new NumericCurrencyCode(756), new Alpha3CurrencyCode("CHF")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(446), new Alpha3CurrencyCode("MOP")),
            new CurrencyCodes(new NumericCurrencyCode(969), new Alpha3CurrencyCode("MGA")),
            new CurrencyCodes(new NumericCurrencyCode(454), new Alpha3CurrencyCode("MWK")),
            new CurrencyCodes(new NumericCurrencyCode(458), new Alpha3CurrencyCode("MYR")),
            new CurrencyCodes(new NumericCurrencyCode(462), new Alpha3CurrencyCode("MVR")),
            new CurrencyCodes(new NumericCurrencyCode(952), new Alpha3CurrencyCode("XOF")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(929), new Alpha3CurrencyCode("MRU")),
            new CurrencyCodes(new NumericCurrencyCode(480), new Alpha3CurrencyCode("MUR")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(965), new Alpha3CurrencyCode("XUA")),
            new CurrencyCodes(new NumericCurrencyCode(484), new Alpha3CurrencyCode("MXN")),
            new CurrencyCodes(new NumericCurrencyCode(979), new Alpha3CurrencyCode("MXV")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(498), new Alpha3CurrencyCode("MDL")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(496), new Alpha3CurrencyCode("MNT")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(951), new Alpha3CurrencyCode("XCD")),
            new CurrencyCodes(new NumericCurrencyCode(504), new Alpha3CurrencyCode("MAD")),
            new CurrencyCodes(new NumericCurrencyCode(943), new Alpha3CurrencyCode("MZN")),
            new CurrencyCodes(new NumericCurrencyCode(104), new Alpha3CurrencyCode("MMK")),
            new CurrencyCodes(new NumericCurrencyCode(516), new Alpha3CurrencyCode("NAD")),
            new CurrencyCodes(new NumericCurrencyCode(710), new Alpha3CurrencyCode("ZAR")),
            new CurrencyCodes(new NumericCurrencyCode(036), new Alpha3CurrencyCode("AUD")),
            new CurrencyCodes(new NumericCurrencyCode(524), new Alpha3CurrencyCode("NPR")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(953), new Alpha3CurrencyCode("XPF")),
            new CurrencyCodes(new NumericCurrencyCode(554), new Alpha3CurrencyCode("NZD")),
            new CurrencyCodes(new NumericCurrencyCode(558), new Alpha3CurrencyCode("NIO")),
            new CurrencyCodes(new NumericCurrencyCode(952), new Alpha3CurrencyCode("XOF")),
            new CurrencyCodes(new NumericCurrencyCode(566), new Alpha3CurrencyCode("NGN")),
            new CurrencyCodes(new NumericCurrencyCode(554), new Alpha3CurrencyCode("NZD")),
            new CurrencyCodes(new NumericCurrencyCode(036), new Alpha3CurrencyCode("AUD")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(578), new Alpha3CurrencyCode("NOK")),
            new CurrencyCodes(new NumericCurrencyCode(512), new Alpha3CurrencyCode("OMR")),
            new CurrencyCodes(new NumericCurrencyCode(586), new Alpha3CurrencyCode("PKR")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(590), new Alpha3CurrencyCode("PAB")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(598), new Alpha3CurrencyCode("PGK")),
            new CurrencyCodes(new NumericCurrencyCode(600), new Alpha3CurrencyCode("PYG")),
            new CurrencyCodes(new NumericCurrencyCode(604), new Alpha3CurrencyCode("PEN")),
            new CurrencyCodes(new NumericCurrencyCode(608), new Alpha3CurrencyCode("PHP")),
            new CurrencyCodes(new NumericCurrencyCode(554), new Alpha3CurrencyCode("NZD")),
            new CurrencyCodes(new NumericCurrencyCode(985), new Alpha3CurrencyCode("PLN")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(840), new Alpha3CurrencyCode("USD")),
            new CurrencyCodes(new NumericCurrencyCode(634), new Alpha3CurrencyCode("QAR")),
            new CurrencyCodes(new NumericCurrencyCode(807), new Alpha3CurrencyCode("MKD")),
            new CurrencyCodes(new NumericCurrencyCode(946), new Alpha3CurrencyCode("RON")),
            new CurrencyCodes(new NumericCurrencyCode(643), new Alpha3CurrencyCode("RUB")),
            new CurrencyCodes(new NumericCurrencyCode(646), new Alpha3CurrencyCode("RWF")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(654), new Alpha3CurrencyCode("SHP")),
            new CurrencyCodes(new NumericCurrencyCode(951), new Alpha3CurrencyCode("XCD")),
            new CurrencyCodes(new NumericCurrencyCode(951), new Alpha3CurrencyCode("XCD")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(951), new Alpha3CurrencyCode("XCD")),
            new CurrencyCodes(new NumericCurrencyCode(882), new Alpha3CurrencyCode("WST")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(930), new Alpha3CurrencyCode("STN")),
            new CurrencyCodes(new NumericCurrencyCode(682), new Alpha3CurrencyCode("SAR")),
            new CurrencyCodes(new NumericCurrencyCode(952), new Alpha3CurrencyCode("XOF")),
            new CurrencyCodes(new NumericCurrencyCode(941), new Alpha3CurrencyCode("RSD")),
            new CurrencyCodes(new NumericCurrencyCode(690), new Alpha3CurrencyCode("SCR")),
            new CurrencyCodes(new NumericCurrencyCode(694), new Alpha3CurrencyCode("SLL")),
            new CurrencyCodes(new NumericCurrencyCode(702), new Alpha3CurrencyCode("SGD")),
            new CurrencyCodes(new NumericCurrencyCode(532), new Alpha3CurrencyCode("ANG")),
            new CurrencyCodes(new NumericCurrencyCode(994), new Alpha3CurrencyCode("XSU")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(978), new Alpha3CurrencyCode("EUR")),
            new CurrencyCodes(new NumericCurrencyCode(090), new Alpha3CurrencyCode("SBD")),
            new CurrencyCodes(new NumericCurrencyCode(706), new Alpha3CurrencyCode("SOS"))
        };

    public static bool IsValid(ushort numericCode)
    {
        return _NumericCurrencyMap.Keys.Any(a => (ushort) a == numericCode);
    }

    public static bool IsValid(ReadOnlySpan<char> alpha3Code)
    {
        if (alpha3Code.Length != 3)
        {
            throw new ArgumentOutOfRangeException(nameof(alpha3Code),
                $"The argument {nameof(alpha3Code)} must be three characters in length");
        }

        _Buffer[0] = alpha3Code[0];
        _Buffer[1] = alpha3Code[1];
        _Buffer[2] = alpha3Code[2];

        return _Alpha3CurrencyMap.Keys.Any(a => a == _Buffer);
    }

    #endregion
}