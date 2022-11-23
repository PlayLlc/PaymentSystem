using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record RewardsProgramActivationHasBeenToggled : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;
    public readonly string UserId;
    public bool IsActive;

    #endregion

    #region Constructor

    public RewardsProgramActivationHasBeenToggled(LoyaltyProgram loyaltyProgram, string userId, bool isActive) : base(
        $"The {nameof(RewardsProgram)} has updated its Activation status to: [{isActive}] by the {nameof(User)} with the ID: [{userId}];")
    {
        LoyaltyProgram = loyaltyProgram;
        UserId = userId;
        IsActive = isActive;
    }

    #endregion
}