using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Play.Globalization.Language;

// HACK: That's a lot of memory. Let's make sure we're using values from the database and not storing these values in code. A Terminal will pull the globalization configuration directly from the database so this is only for testing and stuff 

public static class LanguageCodeRepository
{
    #region Static Metadata

    private static readonly ImmutableDictionary<Alpha2LanguageCode, Language> _Alpha2LanguageCodesMap;
    private static readonly ImmutableDictionary<Alpha3LanguageCode, Language> _Alpha3LanguageCodesMap;

    #endregion

    #region Constructor

    static LanguageCodeRepository()
    {
        HashSet<Language> mapper = new(CreateLanguageCodes());

        _Alpha2LanguageCodesMap = mapper.ToImmutableDictionary(a => a.GetAlpha2Code(), b => b);
        _Alpha3LanguageCodesMap = mapper.ToImmutableDictionary(a => a.GetAlpha3Code(), b => b);
    }

    #endregion

    #region Instance Members

    public static List<Language> GetAll() => _Alpha2LanguageCodesMap.Values.ToList();
    public static bool TryGetAlpha2LanguageCode(Alpha3LanguageCode value, out Language? result) => _Alpha3LanguageCodesMap.TryGetValue(value, out result);
    public static bool TryGetAlpha3LanguageCode(Alpha2LanguageCode value, out Language? result) => _Alpha2LanguageCodesMap.TryGetValue(value, out result);

    private static List<Language> CreateLanguageCodes()
    {
        HashSet<Language> result = new();

        result.Add(new Language(new Alpha2LanguageCode("ab"), new Alpha3LanguageCode("abk"), "Abkhazian"));

        return new List<Language>
        {
            new(new Alpha2LanguageCode("aa"), new Alpha3LanguageCode("aar"), "Afar"),
            new(new Alpha2LanguageCode("af"), new Alpha3LanguageCode("afr"), "Afrikaans"),
            new(new Alpha2LanguageCode("ak"), new Alpha3LanguageCode("aka"), "Akan"),
            new(new Alpha2LanguageCode("sq"), new Alpha3LanguageCode("sqi"), "Albanian"),
            new(new Alpha2LanguageCode("am"), new Alpha3LanguageCode("amh"), "Amharic"),
            new(new Alpha2LanguageCode("ar"), new Alpha3LanguageCode("ara"), "Arabic"),
            new(new Alpha2LanguageCode("an"), new Alpha3LanguageCode("arg"), "Aragonese"),
            new(new Alpha2LanguageCode("hy"), new Alpha3LanguageCode("hye"), "Armenian"),
            new(new Alpha2LanguageCode("as"), new Alpha3LanguageCode("asm"), "Assamese"),
            new(new Alpha2LanguageCode("av"), new Alpha3LanguageCode("ava"), "Avaric"),
            new(new Alpha2LanguageCode("ae"), new Alpha3LanguageCode("ave"), "Avestan"),
            new(new Alpha2LanguageCode("ay"), new Alpha3LanguageCode("aym"), "Aymara"),
            new(new Alpha2LanguageCode("az"), new Alpha3LanguageCode("aze"), "Azerbaijani"),
            new(new Alpha2LanguageCode("bm"), new Alpha3LanguageCode("bam"), "Bambara"),
            new(new Alpha2LanguageCode("ba"), new Alpha3LanguageCode("bak"), "Bashkir"),
            new(new Alpha2LanguageCode("eu"), new Alpha3LanguageCode("eus"), "Basque"),
            new(new Alpha2LanguageCode("be"), new Alpha3LanguageCode("bel"), "Belarusian"),
            new(new Alpha2LanguageCode("bn"), new Alpha3LanguageCode("ben"), "Bengali"),
            new(new Alpha2LanguageCode("bi"), new Alpha3LanguageCode("bis"), "Bislama"),
            new(new Alpha2LanguageCode("bs"), new Alpha3LanguageCode("bos"), "Bosnian"),
            new(new Alpha2LanguageCode("br"), new Alpha3LanguageCode("bre"), "Breton"),
            new(new Alpha2LanguageCode("bg"), new Alpha3LanguageCode("bul"), "Bulgarian"),
            new(new Alpha2LanguageCode("my"), new Alpha3LanguageCode("mya"), "Burmese"),
            new(new Alpha2LanguageCode("ca"), new Alpha3LanguageCode("cat"), "Catalan, Valencian"),
            new(new Alpha2LanguageCode("ch"), new Alpha3LanguageCode("cha"), "Chamorro"),
            new(new Alpha2LanguageCode("ce"), new Alpha3LanguageCode("che"), "Chechen"),
            new(new Alpha2LanguageCode("ny"), new Alpha3LanguageCode("nya"), "Chichewa, Chewa, Nyanja"),
            new(new Alpha2LanguageCode("zh"), new Alpha3LanguageCode("zho"), "Chinese"),
            new(new Alpha2LanguageCode("cv"), new Alpha3LanguageCode("chv"), "Chuvash"),
            new(new Alpha2LanguageCode("kw"), new Alpha3LanguageCode("cor"), "Cornish"),
            new(new Alpha2LanguageCode("co"), new Alpha3LanguageCode("cos"), "Corsican"),
            new(new Alpha2LanguageCode("cr"), new Alpha3LanguageCode("cre"), "Cree"),
            new(new Alpha2LanguageCode("hr"), new Alpha3LanguageCode("hrv"), "Croatian"),
            new(new Alpha2LanguageCode("cs"), new Alpha3LanguageCode("ces"), "Czech"),
            new(new Alpha2LanguageCode("da"), new Alpha3LanguageCode("dan"), "Danish"),
            new(new Alpha2LanguageCode("dv"), new Alpha3LanguageCode("div"), "Divehi, Dhivehi, Maldivian"),
            new(new Alpha2LanguageCode("nl"), new Alpha3LanguageCode("nld"), "Dutch, Flemish"),
            new(new Alpha2LanguageCode("dz"), new Alpha3LanguageCode("dzo"), "Dzongkha"),
            new(new Alpha2LanguageCode("en"), new Alpha3LanguageCode("eng"), "English"),
            new(new Alpha2LanguageCode("eo"), new Alpha3LanguageCode("epo"), "Esperanto"),
            new(new Alpha2LanguageCode("et"), new Alpha3LanguageCode("est"), "Estonian"),
            new(new Alpha2LanguageCode("ee"), new Alpha3LanguageCode("ewe"), "Ewe"),
            new(new Alpha2LanguageCode("fo"), new Alpha3LanguageCode("fao"), "Faroese"),
            new(new Alpha2LanguageCode("fj"), new Alpha3LanguageCode("fij"), "Fijian"),
            new(new Alpha2LanguageCode("fi"), new Alpha3LanguageCode("fin"), "Finnish"),
            new(new Alpha2LanguageCode("fr"), new Alpha3LanguageCode("fra"), "French"),
            new(new Alpha2LanguageCode("fy"), new Alpha3LanguageCode("fry"), "Western Frisian"),
            new(new Alpha2LanguageCode("ff"), new Alpha3LanguageCode("ful"), "Fulah"),
            new(new Alpha2LanguageCode("gd"), new Alpha3LanguageCode("gla"), "Gaelic, Scottish Gaelic"),
            new(new Alpha2LanguageCode("gl"), new Alpha3LanguageCode("glg"), "Galician"),
            new(new Alpha2LanguageCode("lg"), new Alpha3LanguageCode("lug"), "Ganda"),
            new(new Alpha2LanguageCode("ka"), new Alpha3LanguageCode("kat"), "Georgian"),
            new(new Alpha2LanguageCode("de"), new Alpha3LanguageCode("deu"), "German"),
            new(new Alpha2LanguageCode("el"), new Alpha3LanguageCode("ell"), "Greek, Modern (1453–)"),
            new(new Alpha2LanguageCode("kl"), new Alpha3LanguageCode("kal"), "Kalaallisut, Greenlandic"),
            new(new Alpha2LanguageCode("gn"), new Alpha3LanguageCode("grn"), "Guarani"),
            new(new Alpha2LanguageCode("gu"), new Alpha3LanguageCode("guj"), "Gujarati"),
            new(new Alpha2LanguageCode("ht"), new Alpha3LanguageCode("hat"), "Haitian, Haitian Creole"),
            new(new Alpha2LanguageCode("ha"), new Alpha3LanguageCode("hau"), "Hausa"),
            new(new Alpha2LanguageCode("he"), new Alpha3LanguageCode("heb"), "Hebrew"),
            new(new Alpha2LanguageCode("hz"), new Alpha3LanguageCode("her"), "Herero"),
            new(new Alpha2LanguageCode("hi"), new Alpha3LanguageCode("hin"), "Hindi"),
            new(new Alpha2LanguageCode("ho"), new Alpha3LanguageCode("hmo"), "Hiri Motu"),
            new(new Alpha2LanguageCode("hu"), new Alpha3LanguageCode("hun"), "Hungarian"),
            new(new Alpha2LanguageCode("is"), new Alpha3LanguageCode("isl"), "Icelandic"),
            new(new Alpha2LanguageCode("io"), new Alpha3LanguageCode("ido"), "Ido"),
            new(new Alpha2LanguageCode("ig"), new Alpha3LanguageCode("ibo"), "Igbo"),
            new(new Alpha2LanguageCode("id"), new Alpha3LanguageCode("ind"), "Indonesian"),
            new(new Alpha2LanguageCode("ia"), new Alpha3LanguageCode("ina"), "Interlingua (International Auxiliary Language Association)"),
            new(new Alpha2LanguageCode("ie"), new Alpha3LanguageCode("ile"), "Interlingue, Occidental"),
            new(new Alpha2LanguageCode("iu"), new Alpha3LanguageCode("iku"), "Inuktitut"),
            new(new Alpha2LanguageCode("ik"), new Alpha3LanguageCode("ipk"), "Inupiaq"),
            new(new Alpha2LanguageCode("ga"), new Alpha3LanguageCode("gle"), "Irish"),
            new(new Alpha2LanguageCode("it"), new Alpha3LanguageCode("ita"), "Italian"),
            new(new Alpha2LanguageCode("ja"), new Alpha3LanguageCode("jpn"), "Japanese"),
            new(new Alpha2LanguageCode("jv"), new Alpha3LanguageCode("jav"), "Javanese"),
            new(new Alpha2LanguageCode("kn"), new Alpha3LanguageCode("kan"), "Kannada"),
            new(new Alpha2LanguageCode("kr"), new Alpha3LanguageCode("kau"), "Kanuri"),
            new(new Alpha2LanguageCode("ks"), new Alpha3LanguageCode("kas"), "Kashmiri"),
            new(new Alpha2LanguageCode("kk"), new Alpha3LanguageCode("kaz"), "Kazakh"),
            new(new Alpha2LanguageCode("km"), new Alpha3LanguageCode("khm"), "Central Khmer"),
            new(new Alpha2LanguageCode("ki"), new Alpha3LanguageCode("kik"), "Kikuyu, Gikuyu"),
            new(new Alpha2LanguageCode("rw"), new Alpha3LanguageCode("kin"), "Kinyarwanda"),
            new(new Alpha2LanguageCode("ky"), new Alpha3LanguageCode("kir"), "Kirghiz, Kyrgyz"),
            new(new Alpha2LanguageCode("kv"), new Alpha3LanguageCode("kom"), "Komi"),
            new(new Alpha2LanguageCode("kg"), new Alpha3LanguageCode("kon"), "Kongo"),
            new(new Alpha2LanguageCode("ko"), new Alpha3LanguageCode("kor"), "Korean"),
            new(new Alpha2LanguageCode("kj"), new Alpha3LanguageCode("kua"), "Kuanyama, Kwanyama"),
            new(new Alpha2LanguageCode("ku"), new Alpha3LanguageCode("kur"), "Kurdish"),
            new(new Alpha2LanguageCode("lo"), new Alpha3LanguageCode("lao"), "Lao"),
            new(new Alpha2LanguageCode("la"), new Alpha3LanguageCode("lat"), "Latin"),
            new(new Alpha2LanguageCode("lv"), new Alpha3LanguageCode("lav"), "Latvian"),
            new(new Alpha2LanguageCode("li"), new Alpha3LanguageCode("lim"), "Limburgan, Limburger, Limburgish"),
            new(new Alpha2LanguageCode("ln"), new Alpha3LanguageCode("lin"), "Lingala"),
            new(new Alpha2LanguageCode("lt"), new Alpha3LanguageCode("lit"), "Lithuanian"),
            new(new Alpha2LanguageCode("lu"), new Alpha3LanguageCode("lub"), "Luba-Katanga"),
            new(new Alpha2LanguageCode("lb"), new Alpha3LanguageCode("ltz"), "Luxembourgish, Letzeburgesch"),
            new(new Alpha2LanguageCode("mk"), new Alpha3LanguageCode("mkd"), "Macedonian"),
            new(new Alpha2LanguageCode("mg"), new Alpha3LanguageCode("mlg"), "Malagasy"),
            new(new Alpha2LanguageCode("ms"), new Alpha3LanguageCode("msa"), "Malay"),
            new(new Alpha2LanguageCode("ml"), new Alpha3LanguageCode("mal"), "Malayalam"),
            new(new Alpha2LanguageCode("mt"), new Alpha3LanguageCode("mlt"), "Maltese"),
            new(new Alpha2LanguageCode("gv"), new Alpha3LanguageCode("glv"), "Manx"),
            new(new Alpha2LanguageCode("mi"), new Alpha3LanguageCode("mri"), "Maori"),
            new(new Alpha2LanguageCode("mr"), new Alpha3LanguageCode("mar"), "Marathi"),
            new(new Alpha2LanguageCode("mh"), new Alpha3LanguageCode("mah"), "Marshallese"),
            new(new Alpha2LanguageCode("mn"), new Alpha3LanguageCode("mon"), "Mongolian"),
            new(new Alpha2LanguageCode("na"), new Alpha3LanguageCode("nau"), "Nauru"),
            new(new Alpha2LanguageCode("nv"), new Alpha3LanguageCode("nav"), "Navajo, Navaho"),
            new(new Alpha2LanguageCode("nd"), new Alpha3LanguageCode("nde"), "North Ndebele"),
            new(new Alpha2LanguageCode("nr"), new Alpha3LanguageCode("nbl"), "South Ndebele"),
            new(new Alpha2LanguageCode("ng"), new Alpha3LanguageCode("ndo"), "Ndonga"),
            new(new Alpha2LanguageCode("ne"), new Alpha3LanguageCode("nep"), "Nepali"),
            new(new Alpha2LanguageCode("no"), new Alpha3LanguageCode("nor"), "Norwegian"),
            new(new Alpha2LanguageCode("nb"), new Alpha3LanguageCode("nob"), "Norwegian Bokmål"),
            new(new Alpha2LanguageCode("nn"), new Alpha3LanguageCode("nno"), "Norwegian Nynorsk"),
            new(new Alpha2LanguageCode("ii"), new Alpha3LanguageCode("iii"), "Sichuan Yi, Nuosu"),
            new(new Alpha2LanguageCode("oc"), new Alpha3LanguageCode("oci"), "Occitan"),
            new(new Alpha2LanguageCode("oj"), new Alpha3LanguageCode("oji"), "Ojibwa"),
            new(new Alpha2LanguageCode("or"), new Alpha3LanguageCode("ori"), "Oriya"),
            new(new Alpha2LanguageCode("om"), new Alpha3LanguageCode("orm"), "Oromo"),
            new(new Alpha2LanguageCode("os"), new Alpha3LanguageCode("oss"), "Ossetian, Ossetic"),
            new(new Alpha2LanguageCode("pi"), new Alpha3LanguageCode("pli"), "Pali"),
            new(new Alpha2LanguageCode("ps"), new Alpha3LanguageCode("pus"), "Pashto, Pushto"),
            new(new Alpha2LanguageCode("fa"), new Alpha3LanguageCode("fas"), "Persian"),
            new(new Alpha2LanguageCode("pl"), new Alpha3LanguageCode("pol"), "Polish"),
            new(new Alpha2LanguageCode("pt"), new Alpha3LanguageCode("por"), "Portuguese"),
            new(new Alpha2LanguageCode("pa"), new Alpha3LanguageCode("pan"), "Punjabi, Panjabi"),
            new(new Alpha2LanguageCode("qu"), new Alpha3LanguageCode("que"), "Quechua"),
            new(new Alpha2LanguageCode("ro"), new Alpha3LanguageCode("ron"), "Romanian, Moldavian, Moldovan"),
            new(new Alpha2LanguageCode("rm"), new Alpha3LanguageCode("roh"), "Romansh"),
            new(new Alpha2LanguageCode("rn"), new Alpha3LanguageCode("run"), "Rundi"),
            new(new Alpha2LanguageCode("ru"), new Alpha3LanguageCode("rus"), "Russian"),
            new(new Alpha2LanguageCode("se"), new Alpha3LanguageCode("sme"), "Northern Sami"),
            new(new Alpha2LanguageCode("sm"), new Alpha3LanguageCode("smo"), "Samoan"),
            new(new Alpha2LanguageCode("sg"), new Alpha3LanguageCode("sag"), "Sango"),
            new(new Alpha2LanguageCode("sa"), new Alpha3LanguageCode("san"), "Sanskrit"),
            new(new Alpha2LanguageCode("sc"), new Alpha3LanguageCode("srd"), "Sardinian"),
            new(new Alpha2LanguageCode("sr"), new Alpha3LanguageCode("srp"), "Serbian"),
            new(new Alpha2LanguageCode("sn"), new Alpha3LanguageCode("sna"), "Shona"),
            new(new Alpha2LanguageCode("sd"), new Alpha3LanguageCode("snd"), "Sindhi"),
            new(new Alpha2LanguageCode("si"), new Alpha3LanguageCode("sin"), "Sinhala, Sinhalese"),
            new(new Alpha2LanguageCode("sk"), new Alpha3LanguageCode("slk"), "Slovak"),
            new(new Alpha2LanguageCode("sl"), new Alpha3LanguageCode("slv"), "Slovenian"),
            new(new Alpha2LanguageCode("so"), new Alpha3LanguageCode("som"), "Somali"),
            new(new Alpha2LanguageCode("st"), new Alpha3LanguageCode("sot"), "Southern Sotho"),
            new(new Alpha2LanguageCode("es"), new Alpha3LanguageCode("spa"), "Spanish, Castilian"),
            new(new Alpha2LanguageCode("su"), new Alpha3LanguageCode("sun"), "Sundanese"),
            new(new Alpha2LanguageCode("sw"), new Alpha3LanguageCode("swa"), "Swahili"),
            new(new Alpha2LanguageCode("ss"), new Alpha3LanguageCode("ssw"), "Swati"),
            new(new Alpha2LanguageCode("sv"), new Alpha3LanguageCode("swe"), "Swedish"),
            new(new Alpha2LanguageCode("tl"), new Alpha3LanguageCode("tgl"), "Tagalog"),
            new(new Alpha2LanguageCode("ty"), new Alpha3LanguageCode("tah"), "Tahitian"),
            new(new Alpha2LanguageCode("tg"), new Alpha3LanguageCode("tgk"), "Tajik"),
            new(new Alpha2LanguageCode("ta"), new Alpha3LanguageCode("tam"), "Tamil"),
            new(new Alpha2LanguageCode("tt"), new Alpha3LanguageCode("tat"), "Tatar"),
            new(new Alpha2LanguageCode("te"), new Alpha3LanguageCode("tel"), "Telugu"),
            new(new Alpha2LanguageCode("th"), new Alpha3LanguageCode("tha"), "Thai"),
            new(new Alpha2LanguageCode("bo"), new Alpha3LanguageCode("bod"), "Tibetan"),
            new(new Alpha2LanguageCode("ti"), new Alpha3LanguageCode("tir"), "Tigrinya"),
            new(new Alpha2LanguageCode("to"), new Alpha3LanguageCode("ton"), "Tonga (Tonga Islands)"),
            new(new Alpha2LanguageCode("ts"), new Alpha3LanguageCode("tso"), "Tsonga"),
            new(new Alpha2LanguageCode("tn"), new Alpha3LanguageCode("tsn"), "Tswana"),
            new(new Alpha2LanguageCode("tr"), new Alpha3LanguageCode("tur"), "Turkish"),
            new(new Alpha2LanguageCode("tk"), new Alpha3LanguageCode("tuk"), "Turkmen"),
            new(new Alpha2LanguageCode("tw"), new Alpha3LanguageCode("twi"), "Twi"),
            new(new Alpha2LanguageCode("ug"), new Alpha3LanguageCode("uig"), "Uighur, Uyghur"),
            new(new Alpha2LanguageCode("uk"), new Alpha3LanguageCode("ukr"), "Ukrainian"),
            new(new Alpha2LanguageCode("ur"), new Alpha3LanguageCode("urd"), "Urdu"),
            new(new Alpha2LanguageCode("uz"), new Alpha3LanguageCode("uzb"), "Uzbek"),
            new(new Alpha2LanguageCode("ve"), new Alpha3LanguageCode("ven"), "Venda"),
            new(new Alpha2LanguageCode("vi"), new Alpha3LanguageCode("vie"), "Vietnamese"),
            new(new Alpha2LanguageCode("vo"), new Alpha3LanguageCode("vol"), "Volapük"),
            new(new Alpha2LanguageCode("wa"), new Alpha3LanguageCode("wln"), "Walloon"),
            new(new Alpha2LanguageCode("cy"), new Alpha3LanguageCode("cym"), "Welsh"),
            new(new Alpha2LanguageCode("wo"), new Alpha3LanguageCode("wol"), "Wolof"),
            new(new Alpha2LanguageCode("xh"), new Alpha3LanguageCode("xho"), "Xhosa"),
            new(new Alpha2LanguageCode("yi"), new Alpha3LanguageCode("yid"), "Yiddish"),
            new(new Alpha2LanguageCode("yo"), new Alpha3LanguageCode("yor"), "Yoruba"),
            new(new Alpha2LanguageCode("za"), new Alpha3LanguageCode("zha"), "Zhuang, Chuang"),
            new(new Alpha2LanguageCode("zu"), new Alpha3LanguageCode("zul"), "Zulu")
        };
    }

    #endregion
}