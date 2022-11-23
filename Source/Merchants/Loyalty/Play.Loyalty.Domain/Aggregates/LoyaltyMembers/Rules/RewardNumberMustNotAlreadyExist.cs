using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Serviceddds;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates.Rules;

public class RewardNumberMustNotAlreadyExist : BusinessRule<LoyaltyMember, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Rewards)} must be greater than or equal to the amount that is claimed;";

    #endregion

    #region Constructor

    internal RewardNumberMustNotAlreadyExist(IEnsureUniqueRewardNumbers uniqueRewardNumberChecker, string merchantId, string rewardNumber)
    {
        _IsValid = uniqueRewardNumberChecker.IsUnique(merchantId, rewardNumber);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override RewardsNumberIsNotUnique CreateBusinessRuleViolationDomainEvent(LoyaltyMember aggregate) => new(aggregate, this);

    #endregion
}