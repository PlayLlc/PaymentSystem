using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber.Enums;
using Play.Emv.Display;

namespace MockPos.Services
{
    internal class DisplayServiceMock : IDisplayMessages, IDisplayLed
    {
        #region Instance Members

        public Task Display(string displayMessage) => Task.CompletedTask;
        public Task Display(DisplayStatuses displayStatuses) => Task.CompletedTask;

        #endregion
    }
}