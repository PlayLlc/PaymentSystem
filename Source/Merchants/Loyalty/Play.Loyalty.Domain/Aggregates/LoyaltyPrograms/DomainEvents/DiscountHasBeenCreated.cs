using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;
    public readonly Discount Discount;

    #endregion

    #region Constructor

    public DiscountHasBeenCreated(LoyaltyProgram loyaltyProgram, Discount discount, string itemId, string variationId, Money discountPrice) : base(
        $"A {nameof(Discount)} price of {discountPrice.AsLocalFormat()} has been created for the inventory item with ItemId: [{itemId}] and VariationId: [{variationId}]")
    {
        LoyaltyProgram = loyaltyProgram;
        Discount = discount;
    }

    #endregion
}