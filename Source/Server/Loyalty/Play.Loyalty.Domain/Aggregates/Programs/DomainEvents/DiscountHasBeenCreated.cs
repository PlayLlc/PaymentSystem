using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly Programs Programs;
    public readonly Discount Discount;
    public readonly string UserId;

    #endregion

    #region Constructor

    public DiscountHasBeenCreated(Programs programs, Discount discount, string userId, string itemId, string variationId, Money discountPrice) : base(
        $"A {nameof(Discount)} price of {discountPrice.AsLocalFormat()} has been created for the inventory item with ItemId: [{itemId}] and VariationId: [{variationId}]")
    {
        Programs = programs;
        Discount = discount;
        UserId = userId;
    }

    #endregion
}