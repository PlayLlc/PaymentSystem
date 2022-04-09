using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Contracts;
using Play.Globalization.Language;

namespace Play.Emv.Display;

public interface IDisplayMessageRepository
{
    #region Instance Members

    DisplayMessage Get(Alpha2LanguageCode alpha2LanguageCode, MessageIdentifiers messageIdentifiers);

    #endregion
}