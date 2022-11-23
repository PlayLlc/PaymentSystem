using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Aggregates.DomainEvents;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates.Rules;

public class RewardsProgramMustBeActiveToClaimReward : BusinessRule<LoyaltyMember, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(LoyaltyProgram)} must be active to claim a {nameof(Rewards)};";

    #endregion

    #region Constructor

    internal RewardsProgramMustBeActiveToClaimReward(LoyaltyProgram loyaltyProgram)
    {
        _IsValid = loyaltyProgram.IsRewardProgramActive();
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override RewardProgramIsNotActive CreateBusinessRuleViolationDomainEvent(LoyaltyMember aggregate) => new(aggregate, this);

    #endregion
}