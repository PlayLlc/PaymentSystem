using Play.Emv.Ber.DataElements;
using Play.Globalization.Country;

namespace Play.Emv.Display.Configuration
{
    public class DisplayConfiguration
    {
        #region Instance Values

        public readonly DisplayMessages[] DisplayMessages;
        public readonly HoldTimeValue HoldTime;
        public readonly NumericCountryCode CountryCode;

        #endregion

        #region Constructor

        public DisplayConfiguration(DisplayMessages[] messages, HoldTimeValue holdTime)
        {
            DisplayMessages = messages;
        }

        #endregion
    }
}