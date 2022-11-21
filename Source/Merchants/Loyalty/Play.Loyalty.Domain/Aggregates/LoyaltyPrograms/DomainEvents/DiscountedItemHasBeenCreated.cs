using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountedItemHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;

    #endregion

    #region Constructor

    public DiscountedItemHasBeenCreated(LoyaltyProgram loyaltyProgram, string itemId, string variationId, Money discountPrice) : base(
        $"A {nameof(Discount)} price of {discountPrice.AsLocalFormat()} has been created for the inventory item with ItemId: [{itemId}] and VariationId: [{variationId}]")
    {
        LoyaltyProgram = loyaltyProgram;
    }

    #endregion
}