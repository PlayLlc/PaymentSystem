﻿using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates.Rules;

public class RewardsProgramMustBeActiveToClaimReward : BusinessRule<Member, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Programs)} must be active to claim a {nameof(Rewards)};";

    #endregion

    #region Constructor

    internal RewardsProgramMustBeActiveToClaimReward(Programs programs)
    {
        _IsValid = programs.IsRewardProgramActive();
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override RewardProgramIsNotActive CreateBusinessRuleViolationDomainEvent(Member aggregate) => new(aggregate, this);

    #endregion
}