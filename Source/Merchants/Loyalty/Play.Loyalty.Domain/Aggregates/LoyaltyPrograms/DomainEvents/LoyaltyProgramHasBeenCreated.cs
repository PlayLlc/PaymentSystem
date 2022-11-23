using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyProgramHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public LoyaltyProgramHasBeenCreated(LoyaltyProgram loyaltyProgram, string merchantId) : base(
        $"A {nameof(LoyaltyProgram)} has been created with the ID: [{loyaltyProgram.Id}] for The {nameof(Merchant)} with the ID: [{merchantId}]")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}