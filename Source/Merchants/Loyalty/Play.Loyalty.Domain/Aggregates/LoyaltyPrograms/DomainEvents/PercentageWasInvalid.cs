using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Inventory.Domain.Aggregates;

public record PercentageWasInvalid : BrokenRuleOrPolicyDomainEvent<LoyaltyProgram, SimpleStringId>
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public PercentageWasInvalid(LoyaltyProgram loyaltyProgram, IBusinessRule rule) : base(loyaltyProgram, rule)
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}