using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;

namespace Play.Merchants.Onboarding.Contracts.Dto
{
    internal class TerminalDto : Dto<int>
    {
        #region Instance Values

        public override int Id { get; set; }

        #endregion
    }
}