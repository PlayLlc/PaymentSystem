using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record RewardsProgramHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;
    public readonly string UserId;

    #endregion

    #region Constructor

    public RewardsProgramHasBeenUpdated(LoyaltyProgram loyaltyProgram, string userId) : base(
        $"The {nameof(RewardsProgram)} has been updated for {nameof(LoyaltyProgram)} with the ID: {loyaltyProgram.Id};")
    {
        LoyaltyProgram = loyaltyProgram;
        UserId = userId;
    }

    #endregion
}