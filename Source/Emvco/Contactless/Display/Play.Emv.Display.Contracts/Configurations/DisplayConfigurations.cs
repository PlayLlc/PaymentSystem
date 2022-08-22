using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Configuration;

namespace Play.Emv.Reader.Configuration
{
    public class DisplayConfigurations
    {
        #region Instance Values

        private readonly DisplayMessages[] _DisplayMessages;
        private readonly HoldTimeValue _HoldTimeValue;

        #endregion

        #region Constructor

        public DisplayConfigurations(DisplayMessages[] displayMessages, HoldTimeValue holdTimeValue)
        {
            _DisplayMessages = displayMessages;
            _HoldTimeValue = holdTimeValue;
        }

        #endregion

        #region Instance Members

        public DisplayMessages[] GetDisplayMessages() => _DisplayMessages;
        public HoldTimeValue GetHoldTimeValue() => _HoldTimeValue;

        #endregion
    }
}