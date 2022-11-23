using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;
    public readonly Discount Discount;

    #endregion

    #region Constructor

    public DiscountHasBeenUpdated(LoyaltyProgram loyaltyProgram, Discount discount, Money discountPrice) : base(
        $"The {nameof(Discount)} with the ID: [{discount.Id}] has updated its discount price to {discountPrice.AsLocalFormat()}")
    {
        LoyaltyProgram = loyaltyProgram;
        Discount = discount;
    }

    #endregion
}