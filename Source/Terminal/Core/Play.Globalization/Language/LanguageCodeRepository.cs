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

        result.Add(new(new("ab"), new("abk"), "Abkhazian"));

        return new()
        {
            new(new("aa"), new("aar"), "Afar"),
            new(new("af"), new("afr"), "Afrikaans"),
            new(new("ak"), new("aka"), "Akan"),
            new(new("sq"), new("sqi"), "Albanian"),
            new(new("am"), new("amh"), "Amharic"),
            new(new("ar"), new("ara"), "Arabic"),
            new(new("an"), new("arg"), "Aragonese"),
            new(new("hy"), new("hye"), "Armenian"),
            new(new("as"), new("asm"), "Assamese"),
            new(new("av"), new("ava"), "Avaric"),
            new(new("ae"), new("ave"), "Avestan"),
            new(new("ay"), new("aym"), "Aymara"),
            new(new("az"), new("aze"), "Azerbaijani"),
            new(new("bm"), new("bam"), "Bambara"),
            new(new("ba"), new("bak"), "Bashkir"),
            new(new("eu"), new("eus"), "Basque"),
            new(new("be"), new("bel"), "Belarusian"),
            new(new("bn"), new("ben"), "Bengali"),
            new(new("bi"), new("bis"), "Bislama"),
            new(new("bs"), new("bos"), "Bosnian"),
            new(new("br"), new("bre"), "Breton"),
            new(new("bg"), new("bul"), "Bulgarian"),
            new(new("my"), new("mya"), "Burmese"),
            new(new("ca"), new("cat"), "Catalan, Valencian"),
            new(new("ch"), new("cha"), "Chamorro"),
            new(new("ce"), new("che"), "Chechen"),
            new(new("ny"), new("nya"), "Chichewa, Chewa, Nyanja"),
            new(new("zh"), new("zho"), "Chinese"),
            new(new("cv"), new("chv"), "Chuvash"),
            new(new("kw"), new("cor"), "Cornish"),
            new(new("co"), new("cos"), "Corsican"),
            new(new("cr"), new("cre"), "Cree"),
            new(new("hr"), new("hrv"), "Croatian"),
            new(new("cs"), new("ces"), "Czech"),
            new(new("da"), new("dan"), "Danish"),
            new(new("dv"), new("div"), "Divehi, Dhivehi, Maldivian"),
            new(new("nl"), new("nld"), "Dutch, Flemish"),
            new(new("dz"), new("dzo"), "Dzongkha"),
            new(new("en"), new("eng"), "English"),
            new(new("eo"), new("epo"), "Esperanto"),
            new(new("et"), new("est"), "Estonian"),
            new(new("ee"), new("ewe"), "Ewe"),
            new(new("fo"), new("fao"), "Faroese"),
            new(new("fj"), new("fij"), "Fijian"),
            new(new("fi"), new("fin"), "Finnish"),
            new(new("fr"), new("fra"), "French"),
            new(new("fy"), new("fry"), "Western Frisian"),
            new(new("ff"), new("ful"), "Fulah"),
            new(new("gd"), new("gla"), "Gaelic, Scottish Gaelic"),
            new(new("gl"), new("glg"), "Galician"),
            new(new("lg"), new("lug"), "Ganda"),
            new(new("ka"), new("kat"), "Georgian"),
            new(new("de"), new("deu"), "German"),
            new(new("el"), new("ell"), "Greek, Modern (1453–)"),
            new(new("kl"), new("kal"), "Kalaallisut, Greenlandic"),
            new(new("gn"), new("grn"), "Guarani"),
            new(new("gu"), new("guj"), "Gujarati"),
            new(new("ht"), new("hat"), "Haitian, Haitian Creole"),
            new(new("ha"), new("hau"), "Hausa"),
            new(new("he"), new("heb"), "Hebrew"),
            new(new("hz"), new("her"), "Herero"),
            new(new("hi"), new("hin"), "Hindi"),
            new(new("ho"), new("hmo"), "Hiri Motu"),
            new(new("hu"), new("hun"), "Hungarian"),
            new(new("is"), new("isl"), "Icelandic"),
            new(new("io"), new("ido"), "Ido"),
            new(new("ig"), new("ibo"), "Igbo"),
            new(new("id"), new("ind"), "Indonesian"),
            new(new("ia"), new("ina"), "Interlingua (International Auxiliary Language Association)"),
            new(new("ie"), new("ile"), "Interlingue, Occidental"),
            new(new("iu"), new("iku"), "Inuktitut"),
            new(new("ik"), new("ipk"), "Inupiaq"),
            new(new("ga"), new("gle"), "Irish"),
            new(new("it"), new("ita"), "Italian"),
            new(new("ja"), new("jpn"), "Japanese"),
            new(new("jv"), new("jav"), "Javanese"),
            new(new("kn"), new("kan"), "Kannada"),
            new(new("kr"), new("kau"), "Kanuri"),
            new(new("ks"), new("kas"), "Kashmiri"),
            new(new("kk"), new("kaz"), "Kazakh"),
            new(new("km"), new("khm"), "Central Khmer"),
            new(new("ki"), new("kik"), "Kikuyu, Gikuyu"),
            new(new("rw"), new("kin"), "Kinyarwanda"),
            new(new("ky"), new("kir"), "Kirghiz, Kyrgyz"),
            new(new("kv"), new("kom"), "Komi"),
            new(new("kg"), new("kon"), "Kongo"),
            new(new("ko"), new("kor"), "Korean"),
            new(new("kj"), new("kua"), "Kuanyama, Kwanyama"),
            new(new("ku"), new("kur"), "Kurdish"),
            new(new("lo"), new("lao"), "Lao"),
            new(new("la"), new("lat"), "Latin"),
            new(new("lv"), new("lav"), "Latvian"),
            new(new("li"), new("lim"), "Limburgan, Limburger, Limburgish"),
            new(new("ln"), new("lin"), "Lingala"),
            new(new("lt"), new("lit"), "Lithuanian"),
            new(new("lu"), new("lub"), "Luba-Katanga"),
            new(new("lb"), new("ltz"), "Luxembourgish, Letzeburgesch"),
            new(new("mk"), new("mkd"), "Macedonian"),
            new(new("mg"), new("mlg"), "Malagasy"),
            new(new("ms"), new("msa"), "Malay"),
            new(new("ml"), new("mal"), "Malayalam"),
            new(new("mt"), new("mlt"), "Maltese"),
            new(new("gv"), new("glv"), "Manx"),
            new(new("mi"), new("mri"), "Maori"),
            new(new("mr"), new("mar"), "Marathi"),
            new(new("mh"), new("mah"), "Marshallese"),
            new(new("mn"), new("mon"), "Mongolian"),
            new(new("na"), new("nau"), "Nauru"),
            new(new("nv"), new("nav"), "Navajo, Navaho"),
            new(new("nd"), new("nde"), "North Ndebele"),
            new(new("nr"), new("nbl"), "South Ndebele"),
            new(new("ng"), new("ndo"), "Ndonga"),
            new(new("ne"), new("nep"), "Nepali"),
            new(new("no"), new("nor"), "Norwegian"),
            new(new("nb"), new("nob"), "Norwegian Bokmål"),
            new(new("nn"), new("nno"), "Norwegian Nynorsk"),
            new(new("ii"), new("iii"), "Sichuan Yi, Nuosu"),
            new(new("oc"), new("oci"), "Occitan"),
            new(new("oj"), new("oji"), "Ojibwa"),
            new(new("or"), new("ori"), "Oriya"),
            new(new("om"), new("orm"), "Oromo"),
            new(new("os"), new("oss"), "Ossetian, Ossetic"),
            new(new("pi"), new("pli"), "Pali"),
            new(new("ps"), new("pus"), "Pashto, Pushto"),
            new(new("fa"), new("fas"), "Persian"),
            new(new("pl"), new("pol"), "Polish"),
            new(new("pt"), new("por"), "Portuguese"),
            new(new("pa"), new("pan"), "Punjabi, Panjabi"),
            new(new("qu"), new("que"), "Quechua"),
            new(new("ro"), new("ron"), "Romanian, Moldavian, Moldovan"),
            new(new("rm"), new("roh"), "Romansh"),
            new(new("rn"), new("run"), "Rundi"),
            new(new("ru"), new("rus"), "Russian"),
            new(new("se"), new("sme"), "Northern Sami"),
            new(new("sm"), new("smo"), "Samoan"),
            new(new("sg"), new("sag"), "Sango"),
            new(new("sa"), new("san"), "Sanskrit"),
            new(new("sc"), new("srd"), "Sardinian"),
            new(new("sr"), new("srp"), "Serbian"),
            new(new("sn"), new("sna"), "Shona"),
            new(new("sd"), new("snd"), "Sindhi"),
            new(new("si"), new("sin"), "Sinhala, Sinhalese"),
            new(new("sk"), new("slk"), "Slovak"),
            new(new("sl"), new("slv"), "Slovenian"),
            new(new("so"), new("som"), "Somali"),
            new(new("st"), new("sot"), "Southern Sotho"),
            new(new("es"), new("spa"), "Spanish, Castilian"),
            new(new("su"), new("sun"), "Sundanese"),
            new(new("sw"), new("swa"), "Swahili"),
            new(new("ss"), new("ssw"), "Swati"),
            new(new("sv"), new("swe"), "Swedish"),
            new(new("tl"), new("tgl"), "Tagalog"),
            new(new("ty"), new("tah"), "Tahitian"),
            new(new("tg"), new("tgk"), "Tajik"),
            new(new("ta"), new("tam"), "Tamil"),
            new(new("tt"), new("tat"), "Tatar"),
            new(new("te"), new("tel"), "Telugu"),
            new(new("th"), new("tha"), "Thai"),
            new(new("bo"), new("bod"), "Tibetan"),
            new(new("ti"), new("tir"), "Tigrinya"),
            new(new("to"), new("ton"), "Tonga (Tonga Islands)"),
            new(new("ts"), new("tso"), "Tsonga"),
            new(new("tn"), new("tsn"), "Tswana"),
            new(new("tr"), new("tur"), "Turkish"),
            new(new("tk"), new("tuk"), "Turkmen"),
            new(new("tw"), new("twi"), "Twi"),
            new(new("ug"), new("uig"), "Uighur, Uyghur"),
            new(new("uk"), new("ukr"), "Ukrainian"),
            new(new("ur"), new("urd"), "Urdu"),
            new(new("uz"), new("uzb"), "Uzbek"),
            new(new("ve"), new("ven"), "Venda"),
            new(new("vi"), new("vie"), "Vietnamese"),
            new(new("vo"), new("vol"), "Volapük"),
            new(new("wa"), new("wln"), "Walloon"),
            new(new("cy"), new("cym"), "Welsh"),
            new(new("wo"), new("wol"), "Wolof"),
            new(new("xh"), new("xho"), "Xhosa"),
            new(new("yi"), new("yid"), "Yiddish"),
            new(new("yo"), new("yor"), "Yoruba"),
            new(new("za"), new("zha"), "Zhuang, Chuang"),
            new(new("zu"), new("zul"), "Zulu")
        };
    }

    #endregion
}