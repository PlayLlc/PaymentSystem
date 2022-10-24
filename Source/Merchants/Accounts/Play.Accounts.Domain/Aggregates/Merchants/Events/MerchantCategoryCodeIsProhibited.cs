using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates
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