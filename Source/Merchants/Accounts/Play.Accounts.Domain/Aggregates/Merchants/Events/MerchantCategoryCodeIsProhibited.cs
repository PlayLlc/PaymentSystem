using Play.Domain.Aggregates;
using Play.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Domain.Aggregates.Merchants.Events
{
    public record MerchantCategoryCodeIsProhibited : BrokenBusinessRuleDomainEvent<Merchant, string>
    {
        #region Instance Values

        public readonly Merchant Merchant;

        #endregion

        #region Constructor

        public MerchantCategoryCodeIsProhibited(Merchant merchant, IBusinessRule rule) : base(merchant, rule)
        {
            Merchant = merchant;
        }

        #endregion
    }
}