using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record RewardsNumberIsNotUnique : BrokenRuleOrPolicyDomainEvent<LoyaltyMember, SimpleStringId>
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;

    #endregion

    #region Constructor

    public RewardsNumberIsNotUnique(LoyaltyMember loyaltyMember, IBusinessRule rule) : base(loyaltyMember, rule)
    {
        LoyaltyMember = loyaltyMember;
    }

    #endregion
}