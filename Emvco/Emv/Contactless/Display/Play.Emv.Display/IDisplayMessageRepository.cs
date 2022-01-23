using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Globalization.Language;

namespace Play.Emv.Display;

public interface IDisplayMessageRepository
{
    #region Instance Members

    DisplayMessage Get(Alpha2LanguageCode alpha2LanguageCode, MessageIdentifier messageIdentifier);

    #endregion
}