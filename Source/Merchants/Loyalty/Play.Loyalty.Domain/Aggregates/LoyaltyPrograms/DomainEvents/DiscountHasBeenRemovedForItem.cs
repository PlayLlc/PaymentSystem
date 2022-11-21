using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountHasBeenRemovedForItem : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public DiscountHasBeenRemovedForItem(LoyaltyProgram loyaltyProgram, string discountId) : base(
        $"The {nameof(Discount)} with the ID: [{discountId}] has been removed")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}