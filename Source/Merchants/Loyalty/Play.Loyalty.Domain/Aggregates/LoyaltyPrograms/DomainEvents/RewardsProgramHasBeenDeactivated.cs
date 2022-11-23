using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record RewardsProgramHasBeenDeactivated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public RewardsProgramHasBeenDeactivated(LoyaltyProgram loyaltyProgram) : base(
        $"The {nameof(RewardsProgram)} has been Deactivated for the {nameof(LoyaltyProgram)} with the ID: [{loyaltyProgram.Id}]")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}