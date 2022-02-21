using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Globalization;

namespace Play.Emv.Display.Contracts;

public class DisplayMessages
{
    #region Instance Values

    private readonly CultureProfile _CultureProfile;
    private readonly ImmutableSortedDictionary<MessageIdentifier, DisplayMessage> _DisplayMessages;

    #endregion

    #region Constructor

    public DisplayMessages(
        LanguagePreference languagePreference,
        TerminalCountryCode terminalCountryCode,
        Dictionary<MessageIdentifier, DisplayMessage> displayMessages)
    {
        _CultureProfile = new CultureProfile(terminalCountryCode.AsCountryCode(), languagePreference.GetPreferredLanguage());
        CheckCore.ForEmptySequence(displayMessages, nameof(displayMessages));
        _DisplayMessages = displayMessages.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    public DisplayMessages(CultureProfile cultureProfile, Dictionary<MessageIdentifier, DisplayMessage> displayMessages)
    {
        _CultureProfile = cultureProfile;
        CheckCore.ForEmptySequence(displayMessages, nameof(displayMessages));
        _DisplayMessages = displayMessages.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    #endregion

    #region Instance Members

    public CultureProfile GetCultureProfile() => throw new NotImplementedException();
    public DisplayMessage Get(MessageIdentifier messageIdentifier) => _DisplayMessages[messageIdentifier];
    public LanguagePreference GetLanguagePreference() => new(_CultureProfile.GetAlpha2LanguageCode());
    public TerminalCountryCode GeTerminalCountryCode() => new(_CultureProfile.GetNumericCountryCode());

    #endregion
}