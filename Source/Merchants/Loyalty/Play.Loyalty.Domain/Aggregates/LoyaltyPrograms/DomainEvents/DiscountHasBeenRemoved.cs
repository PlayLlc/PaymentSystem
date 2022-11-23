using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;
    public readonly Discount Discount;

    #endregion

    #region Constructor

    public DiscountHasBeenRemoved(LoyaltyProgram loyaltyProgram, Discount discount) : base(
        $"The {nameof(Discount)} with the ID: [{discount.Id}] has been removed")
    {
        LoyaltyProgram = loyaltyProgram;
        Discount = discount;
    }

    #endregion
}