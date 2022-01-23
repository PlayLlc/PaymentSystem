using System;
using System.Collections.Generic;

namespace Play.Globalization.Language;

public class LanguageCodes : IEqualityComparer<LanguageCodes>, IEquatable<LanguageCodes>
{
    #region Instance Values

    private readonly Alpha2LanguageCode _Alpha2;
    private readonly Alpha3LanguageCode _Alpha3;

    #endregion

    #region Constructor

    public LanguageCodes(Alpha2LanguageCode alpha2, Alpha3LanguageCode alpha3)
    {
        _Alpha2 = alpha2;
        _Alpha3 = alpha3;
    }

    #endregion

    #region Instance Members

    public Alpha3LanguageCode GetAlpha2Code() => _Alpha3;
    public Alpha2LanguageCode GetAlpha2LanguageCode() => _Alpha2;

    #endregion

    #region Equality

    public bool Equals(LanguageCodes? x, LanguageCodes? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(LanguageCodes? other)
    {
        if (other == null)
            return false;

        return (_Alpha3 == other._Alpha3) && (_Alpha2 == other._Alpha2);
    }

    public override bool Equals(object other) => other is LanguageCodes languageCodes && Equals(languageCodes);
    public int GetHashCode(LanguageCodes obj) => obj.GetHashCode();
    public override int GetHashCode() => _Alpha3.GetHashCode() + _Alpha2.GetHashCode();

    #endregion
}