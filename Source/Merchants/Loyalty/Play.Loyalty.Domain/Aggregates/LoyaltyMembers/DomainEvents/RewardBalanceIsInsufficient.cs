using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Inventory.Domain.Aggregates;

public record RewardBalanceIsInsufficient : BrokenRuleOrPolicyDomainEvent<LoyaltyMember, SimpleStringId>
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;

    #endregion

    #region Constructor

    public RewardBalanceIsInsufficient(LoyaltyMember loyaltyMember, IBusinessRule rule) : base(loyaltyMember, rule)
    {
        LoyaltyMember = loyaltyMember;
    }

    #endregion
}