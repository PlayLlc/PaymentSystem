using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Aggregates.DomainEvents;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates.Rules;

public class RewardMustBeGreaterThanOrEqualToClaimAmount : BusinessRule<LoyaltyMember, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Rewards)} must be greater than or equal to the amount that is claimed;";

    #endregion

    #region Constructor

    internal RewardMustBeGreaterThanOrEqualToClaimAmount(Money amountToClaim, Money rewardsBalance)
    {
        _IsValid = rewardsBalance >= amountToClaim;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override RewardBalanceIsInsufficient CreateBusinessRuleViolationDomainEvent(LoyaltyMember aggregate) => new(aggregate, this);

    #endregion
}