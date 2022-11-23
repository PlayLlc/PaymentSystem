using Play.Domain.Events;
using Play.Loyalty.Domain.Entitiesddd;

namespace Play.Loyalty.Domain.Aggregates;

public record RewardsProgramHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public RewardsProgramHasBeenUpdated(LoyaltyProgram loyaltyProgram) : base(
        $"The {nameof(RewardsProgram)} has been updated for {nameof(LoyaltyProgram)} with the ID: {loyaltyProgram.Id};")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}