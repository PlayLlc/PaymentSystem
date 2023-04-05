using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record MerchantCategoryCodeIsProhibited : BrokenRuleOrPolicyDomainEvent<Merchant, SimpleStringId>
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