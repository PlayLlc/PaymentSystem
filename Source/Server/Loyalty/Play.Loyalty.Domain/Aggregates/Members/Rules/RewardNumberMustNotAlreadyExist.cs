using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Aggregates.Rules;

public class RewardNumberMustNotAlreadyExist : BusinessRule<Member>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Rewards)} must be greater than or equal to the amount that is claimed;";

    #endregion

    #region Constructor

    internal RewardNumberMustNotAlreadyExist(IEnsureRewardsNumbersAreUnique uniqueRewardNumberChecker, string merchantId, string rewardNumber)
    {
        _IsValid = uniqueRewardNumberChecker.IsRewardsNumberUnique(new SimpleStringId(merchantId), rewardNumber);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override RewardsNumberIsNotUnique CreateBusinessRuleViolationDomainEvent(Member aggregate) => new(aggregate, this);

    #endregion
}