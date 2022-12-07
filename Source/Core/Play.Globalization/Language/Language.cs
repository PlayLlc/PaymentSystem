using System;
using System.Collections.Generic;

namespace Play.Globalization.Language;

public class Language : IEqualityComparer<Language>, IEquatable<Language>
{
    #region Instance Values

    private readonly Alpha2LanguageCode _Alpha2;
    private readonly Alpha3LanguageCode _Alpha3;
    private readonly string _Name;

    #endregion

    #region Constructor

    internal Language(Alpha2LanguageCode alpha2, Alpha3LanguageCode alpha3, string name)
    {
        _Alpha2 = alpha2;
        _Alpha3 = alpha3;
        _Name = name;
    }

    #endregion

    #region Instance Members

    public Alpha3LanguageCode GetAlpha3Code() => _Alpha3;
    public Alpha2LanguageCode GetAlpha2Code() => _Alpha2;

    #endregion

    #region Equality

    public bool Equals(Language? x, Language? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(Language? other)
    {
        if (other == null)
            return false;

        return (_Alpha3 == other._Alpha3) && (_Alpha2 == other._Alpha2);
    }

    public override bool Equals(object other) => other is Language languageCodes && Equals(languageCodes);
    public int GetHashCode(Language obj) => obj.GetHashCode();
    public override int GetHashCode() => _Alpha3.GetHashCode() + _Alpha2.GetHashCode();

    #endregion
}