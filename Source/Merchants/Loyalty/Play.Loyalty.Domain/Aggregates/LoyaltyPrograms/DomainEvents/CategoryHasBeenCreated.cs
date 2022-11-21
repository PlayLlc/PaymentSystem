using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Entitiesddd;
using Play.Loyalty.Domain.Entitiessss;
using Play.Loyalty.Domain.Enums;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

public record PercentageOffHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public PercentageOffHasBeenUpdated(LoyaltyProgram loyaltyProgram, ulong percentageOff, string merchantId) : base(
        $"The {nameof(Merchant)} with the ID: [{merchantId}] has updated the {nameof(LoyaltyProgram)} {nameof(PercentageOff)} to {percentageOff};")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}

public record AmountOffHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public AmountOffHasBeenUpdated(LoyaltyProgram loyaltyProgram, Money amountOff) : base(
        $"The {nameof(LoyaltyProgram)} with the ID: [{loyaltyProgram.Id}] has updated its {nameof(AmountOff)} to {amountOff.AsLocalFormat()};")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}

public record RewardProgramActivationHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public RewardProgramActivationHasBeenUpdated(LoyaltyProgram loyaltyProgram, bool isActive) : base(isActive
        ? $"The {nameof(RewardProgram)} has been Activated for the {nameof(LoyaltyProgram)} with the ID: [{loyaltyProgram.Id}]"
        : $"The {nameof(RewardProgram)} has been Deactivated for the {nameof(LoyaltyProgram)} with the ID: [{loyaltyProgram.Id}]")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}

public record RewardTypeHasBeenSet : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public RewardTypeHasBeenSet(LoyaltyProgram loyaltyProgram, RewardTypes rewardType) : base(
        $"The {nameof(LoyaltyProgram)} has updated its {nameof(RewardType)} to: [{rewardType}]")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}