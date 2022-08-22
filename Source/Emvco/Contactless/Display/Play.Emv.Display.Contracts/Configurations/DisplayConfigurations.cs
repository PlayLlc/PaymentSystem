using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Configuration;
using Play.Globalization.Country;

namespace Play.Emv.Reader.Configuration
{
    public class DisplayConfigurations
    {
        #region Instance Values

        private readonly DisplayMessages[] _DisplayMessages;
        private readonly HoldTimeValue _HoldTimeValue;
        private readonly NumericCountryCode _CountryCode;

        #endregion

        #region Constructor

        public DisplayConfigurations(DisplayMessages[] displayMessages, HoldTimeValue holdTimeValue, NumericCountryCode countryCode)
        {
            _DisplayMessages = displayMessages;
            _HoldTimeValue = holdTimeValue;
            _CountryCode = countryCode;
        }

        #endregion

        #region Instance Members

        public NumericCountryCode GetNumericCountryCode() => _CountryCode;
        public DisplayMessages[] GetDisplayMessages() => _DisplayMessages;
        public HoldTimeValue GetHoldTimeValue() => _HoldTimeValue;

        #endregion
    }
}