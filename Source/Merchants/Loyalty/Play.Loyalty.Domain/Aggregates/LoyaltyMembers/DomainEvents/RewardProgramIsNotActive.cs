using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates.DomainEvents;

public record RewardProgramIsNotActive : BrokenRuleOrPolicyDomainEvent<LoyaltyMember, SimpleStringId>
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;

    #endregion

    #region Constructor

    public RewardProgramIsNotActive(LoyaltyMember loyaltyMember, IBusinessRule rule) : base(loyaltyMember, rule)
    {
        LoyaltyMember = loyaltyMember;
    }

    #endregion
}