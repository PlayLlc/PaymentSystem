using System.Collections.Immutable;

using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Configuration;
using Play.Emv.Reader.Configuration;
using Play.Globalization;
using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;

namespace Play.Emv.Display
{
    public class DisplayFormatter : IFormatDisplayMessages
    {
        #region Instance Values

        private readonly ImmutableSortedDictionary<Alpha2LanguageCode, DisplayMessages> _DisplayMessages;
        private readonly NumericCountryCode _CountryCode;

        #endregion

        #region Constructor

        public DisplayFormatter(DisplayConfigurations displayConfiguration)
        {
            _DisplayMessages = displayConfiguration.GetDisplayMessages().ToImmutableSortedDictionary(a => a.GetLanguageCode(), b => b);
            _CountryCode = displayConfiguration.GetNumericCountryCode();
        }

        #endregion

        #region Instance Members

        public string Display(UserInterfaceRequestData userInterfaceRequestData)
        {
            DisplayMessage displayMessage = _DisplayMessages[userInterfaceRequestData.GetLanguagePreference().GetPreferredLanguage()]
                .GetDisplayMessage(userInterfaceRequestData.GetMessageIdentifier());

            if (!userInterfaceRequestData.IsValueQualifierPresent())
                return displayMessage.Display();

            Money money = userInterfaceRequestData.GetAmount()!;
            CultureProfile culture = new(_CountryCode, userInterfaceRequestData.GetLanguagePreference().GetPreferredLanguage());

            return money.AsLocalFormat(culture);
        }

        #endregion
    }
}