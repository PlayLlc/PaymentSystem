using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Emv.Ber.ValueTypes;
using Play.Globalization.Country;
using Play.Globalization.Language;

namespace Play.Emv.Display.Configuration;

public class DisplayMessages
{
    #region Instance Values

    private readonly Alpha2LanguageCode _LanguageCode;
    private readonly ImmutableSortedDictionary<DisplayMessageIdentifier, DisplayMessage> _DisplayMessages;

    #endregion

    #region Constructor

    public DisplayMessages(Alpha2LanguageCode languageCode, Dictionary<DisplayMessageIdentifier, DisplayMessage> displayMessages)
    {
        _LanguageCode = languageCode;
        _DisplayMessages = displayMessages.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public Alpha2LanguageCode GetLanguageCode() => _LanguageCode;
    public DisplayMessage GetDisplayMessage(DisplayMessageIdentifier displayMessageIdentifiers) => _DisplayMessages[displayMessageIdentifiers];

    #endregion
}