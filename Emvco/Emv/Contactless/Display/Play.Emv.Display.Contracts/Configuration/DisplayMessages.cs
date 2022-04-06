using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Globalization;

namespace Play.Emv.Display.Contracts;

public class DisplayMessages
{
    #region Instance Values

    private readonly CultureProfile _CultureProfile;
    private readonly ImmutableSortedDictionary<MessageIdentifiers, DisplayMessage> _DisplayMessages;

    #endregion

    #region Constructor

    public DisplayMessages(
        LanguagePreference languagePreference, TerminalCountryCode terminalCountryCode,
        Dictionary<MessageIdentifiers, DisplayMessage> displayMessages)
    {
        _CultureProfile = new CultureProfile(terminalCountryCode.AsCountryCode(), languagePreference.GetPreferredLanguage());
        CheckCore.ForEmptySequence(displayMessages, nameof(displayMessages));
        _DisplayMessages = displayMessages.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    public DisplayMessages(CultureProfile cultureProfile, Dictionary<MessageIdentifiers, DisplayMessage> displayMessages)
    {
        _CultureProfile = cultureProfile;
        CheckCore.ForEmptySequence(displayMessages, nameof(displayMessages));
        _DisplayMessages = displayMessages.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    #endregion

    #region Instance Members

    public CultureProfile GetCultureProfile() => throw new NotImplementedException();
    public DisplayMessage Get(MessageIdentifiers messageIdentifiers) => _DisplayMessages[messageIdentifiers];
    public LanguagePreference GetLanguagePreference() => new(_CultureProfile.GetAlpha2LanguageCode());
    public TerminalCountryCode GeTerminalCountryCode() => new(_CultureProfile.GetNumericCountryCode());

    #endregion
}