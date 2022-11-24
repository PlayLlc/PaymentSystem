﻿using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountedPriceIsTooHigh : BrokenRuleOrPolicyDomainEvent<LoyaltyProgram, SimpleStringId>
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public DiscountedPriceIsTooHigh(LoyaltyProgram loyaltyProgram, IBusinessRule rule) : base(loyaltyProgram, rule)
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}