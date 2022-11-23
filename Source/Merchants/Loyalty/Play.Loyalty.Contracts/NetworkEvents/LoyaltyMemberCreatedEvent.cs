using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Contracts.NetworkEvents
{
    public class LoyaltyMemberCreatedEvent
    {
        #region Instance Values

        public LoyaltyMemberDto LoyaltyMember { get; set; }

        #endregion
    }

    public class LoyaltyMemberRemovedEvent
    {
        #region Instance Values

        public LoyaltyMemberDto LoyaltyMember { get; set; }

        #endregion
    }

    public class LoyaltyMemberUpdatedEvent
    {
        #region Instance Values

        public LoyaltyMemberDto LoyaltyMember { get; set; }

        #endregion
    }
}