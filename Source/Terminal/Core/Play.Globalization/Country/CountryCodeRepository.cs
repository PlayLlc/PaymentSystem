﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Play.Globalization.Country;

// TODO: Inherit from EnumObject<CountryCode> so the user can specifically pick a country.  Maybe turn
// TODO: CountryCode into a struct
internal class CountryCodeRepository
{
    #region Static Metadata

    private static readonly ImmutableDictionary<Alpha2CountryCode, CountryCodes> _Alpha2CountryMap;
    private static readonly ImmutableDictionary<NumericCountryCode, CountryCodes> _NumericCountryMap;
    private static readonly char[] _Buffer = new char[2];

    #endregion

    #region Constructor

    static CountryCodeRepository()
    {
        _NumericCountryMap = GetCurrencyCodes().ToImmutableDictionary(a => a.GetNumericCode(), b => b);
        _Alpha2CountryMap = GetCurrencyCodes().ToImmutableDictionary(a => a.GetAlpha2Code(), b => b);
    }

    #endregion

    #region Instance Members

    public static CountryCodes Get(NumericCountryCode numericCode) => _NumericCountryMap[numericCode];
    public static CountryCodes Get(Alpha2CountryCode alphaCode) => _Alpha2CountryMap[alphaCode];

    private static List<CountryCodes> GetCurrencyCodes() =>
        new()
        {
            new(new(4), new("AF")),
            new(new(248), new("AX")),
            new(new(8), new("AL")),
            new(new(12), new("DZ")),
            new(new(16), new("AS")),
            new(new(20), new("AD")),
            new(new(24), new("AO")),
            new(new(660), new("AI")),
            new(new(10), new("AQ")),
            new(new(28), new("AG")),
            new(new(32), new("AR")),
            new(new(51), new("AM")),
            new(new(533), new("AW")),
            new(new(36), new("AU")),
            new(new(40), new("AT")),
            new(new(31), new("AZ")),
            new(new(44), new("BS")),
            new(new(48), new("BH")),
            new(new(50), new("BD")),
            new(new(52), new("BB")),
            new(new(112), new("BY")),
            new(new(56), new("BE")),
            new(new(84), new("BZ")),
            new(new(204), new("BJ")),
            new(new(60), new("BM")),
            new(new(64), new("BT")),
            new(new(68), new("BO")),
            new(new(70), new("BA")),
            new(new(72), new("BW")),
            new(new(74), new("BV")),
            new(new(76), new("BR")),
            new(new(92), new("VG")),
            new(new(86), new("IO")),
            new(new(96), new("BN")),
            new(new(100), new("BG")),
            new(new(854), new("BF")),
            new(new(108), new("BI")),
            new(new(116), new("KH")),
            new(new(120), new("CM")),
            new(new(124), new("CA")),
            new(new(132), new("CV")),
            new(new(136), new("KY")),
            new(new(140), new("CF")),
            new(new(148), new("TD")),
            new(new(152), new("CL")),
            new(new(156), new("CN")),
            new(new(344), new("HK")),
            new(new(446), new("MO")),
            new(new(162), new("CX")),
            new(new(166), new("CC")),
            new(new(170), new("CO")),
            new(new(174), new("KM")),
            new(new(178), new("CG")),
            new(new(180), new("CD")),
            new(new(184), new("CK")),
            new(new(188), new("CR")),
            new(new(384), new("CI")),
            new(new(191), new("HR")),
            new(new(192), new("CU")),
            new(new(196), new("CY")),
            new(new(203), new("CZ")),
            new(new(208), new("DK")),
            new(new(262), new("DJ")),
            new(new(212), new("DM")),
            new(new(214), new("DO")),
            new(new(218), new("EC")),
            new(new(818), new("EG")),
            new(new(222), new("SV")),
            new(new(226), new("GQ")),
            new(new(232), new("ER")),
            new(new(233), new("EE")),
            new(new(231), new("ET")),
            new(new(238), new("FK")),
            new(new(234), new("FO")),
            new(new(242), new("FJ")),
            new(new(246), new("FI")),
            new(new(250), new("FR")),
            new(new(254), new("GF")),
            new(new(258), new("PF")),
            new(new(260), new("TF")),
            new(new(266), new("GA")),
            new(new(270), new("GM")),
            new(new(268), new("GE")),
            new(new(276), new("DE")),
            new(new(288), new("GH")),
            new(new(292), new("GI")),
            new(new(300), new("GR")),
            new(new(304), new("GL")),
            new(new(308), new("GD")),
            new(new(312), new("GP")),
            new(new(316), new("GU")),
            new(new(320), new("GT")),
            new(new(831), new("GG")),
            new(new(324), new("GN")),
            new(new(624), new("GW")),
            new(new(328), new("GY")),
            new(new(332), new("HT")),
            new(new(334), new("HM")),
            new(new(336), new("VA")),
            new(new(340), new("HN")),
            new(new(348), new("HU")),
            new(new(352), new("IS")),
            new(new(356), new("IN")),
            new(new(360), new("ID")),
            new(new(364), new("IR")),
            new(new(368), new("IQ")),
            new(new(372), new("IE")),
            new(new(833), new("IM")),
            new(new(376), new("IL")),
            new(new(380), new("IT")),
            new(new(388), new("JM")),
            new(new(392), new("JP")),
            new(new(832), new("JE")),
            new(new(400), new("JO")),
            new(new(398), new("KZ")),
            new(new(404), new("KE")),
            new(new(296), new("KI")),
            new(new(408), new("KP")),
            new(new(410), new("KR")),
            new(new(414), new("KW")),
            new(new(417), new("KG")),
            new(new(418), new("LA")),
            new(new(428), new("LV")),
            new(new(422), new("LB")),
            new(new(426), new("LS")),
            new(new(430), new("LR")),
            new(new(434), new("LY")),
            new(new(438), new("LI")),
            new(new(440), new("LT")),
            new(new(442), new("LU")),
            new(new(807), new("MK")),
            new(new(450), new("MG")),
            new(new(454), new("MW")),
            new(new(458), new("MY")),
            new(new(462), new("MV")),
            new(new(466), new("ML")),
            new(new(470), new("MT")),
            new(new(584), new("MH")),
            new(new(474), new("MQ")),
            new(new(478), new("MR")),
            new(new(480), new("MU")),
            new(new(175), new("YT")),
            new(new(484), new("MX")),
            new(new(583), new("FM")),
            new(new(498), new("MD")),
            new(new(492), new("MC")),
            new(new(496), new("MN")),
            new(new(499), new("ME")),
            new(new(500), new("MS")),
            new(new(504), new("MA")),
            new(new(508), new("MZ")),
            new(new(104), new("MM")),
            new(new(516), new("NA")),
            new(new(520), new("NR")),
            new(new(524), new("NP")),
            new(new(528), new("NL")),
            new(new(530), new("AN")),
            new(new(540), new("NC")),
            new(new(554), new("NZ")),
            new(new(558), new("NI")),
            new(new(562), new("NE")),
            new(new(566), new("NG")),
            new(new(570), new("NU")),
            new(new(574), new("NF")),
            new(new(580), new("MP")),
            new(new(578), new("NO")),
            new(new(512), new("OM")),
            new(new(586), new("PK")),
            new(new(585), new("PW")),
            new(new(275), new("PS")),
            new(new(591), new("PA")),
            new(new(598), new("PG")),
            new(new(600), new("PY")),
            new(new(604), new("PE")),
            new(new(608), new("PH")),
            new(new(612), new("PN")),
            new(new(616), new("PL")),
            new(new(620), new("PT")),
            new(new(630), new("PR")),
            new(new(634), new("QA")),
            new(new(638), new("RE")),
            new(new(642), new("RO")),
            new(new(643), new("RU")),
            new(new(646), new("RW")),
            new(new(652), new("BL")),
            new(new(654), new("SH")),
            new(new(659), new("KN")),
            new(new(662), new("LC")),
            new(new(663), new("MF")),
            new(new(666), new("PM")),
            new(new(670), new("VC")),
            new(new(882), new("WS")),
            new(new(674), new("SM")),
            new(new(678), new("ST")),
            new(new(682), new("SA")),
            new(new(686), new("SN")),
            new(new(688), new("RS")),
            new(new(690), new("SC")),
            new(new(694), new("SL")),
            new(new(702), new("SG")),
            new(new(703), new("SK")),
            new(new(705), new("SI")),
            new(new(90), new("SB")),
            new(new(706), new("SO")),
            new(new(710), new("ZA")),
            new(new(239), new("GS")),
            new(new(728), new("SS")),
            new(new(724), new("ES")),
            new(new(144), new("LK")),
            new(new(736), new("SD")),
            new(new(740), new("SR")),
            new(new(744), new("SJ")),
            new(new(748), new("SZ")),
            new(new(752), new("SE")),
            new(new(756), new("CH")),
            new(new(760), new("SY")),
            new(new(158), new("TW")),
            new(new(762), new("TJ")),
            new(new(834), new("TZ")),
            new(new(764), new("TH")),
            new(new(626), new("TL")),
            new(new(768), new("TG")),
            new(new(772), new("TK")),
            new(new(776), new("TO")),
            new(new(780), new("TT")),
            new(new(788), new("TN")),
            new(new(792), new("TR")),
            new(new(795), new("TM")),
            new(new(796), new("TC")),
            new(new(798), new("TV")),
            new(new(800), new("UG")),
            new(new(804), new("UA")),
            new(new(784), new("AE")),
            new(new(826), new("GB")),
            new(new(840), new("US")),
            new(new(581), new("UM")),
            new(new(858), new("UY")),
            new(new(860), new("UZ")),
            new(new(548), new("VU")),
            new(new(862), new("VE")),
            new(new(704), new("VN")),
            new(new(850), new("VI")),
            new(new(876), new("WF")),
            new(new(732), new("EH")),
            new(new(887), new("YE")),
            new(new(894), new("ZM")),
            new(new(716), new("ZW"))
        };

    public static bool IsValid(ushort numericCode)
    {
        return _NumericCountryMap.Keys.Any(a => a == numericCode);
    }

    public static bool IsValid(ReadOnlySpan<char> alpha2Code)
    {
        if (alpha2Code.Length != 2)
            throw new ArgumentOutOfRangeException(nameof(alpha2Code), $"The argument {nameof(alpha2Code)} must be two characters in length");

        char[] buffer = alpha2Code.ToArray();

        return _Alpha2CountryMap.Keys.Any(a => a.Equals(buffer));
    }

    #endregion
}