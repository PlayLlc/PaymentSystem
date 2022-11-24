using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;
    public readonly string DiscountId;

    #endregion

    #region Constructor

    public DiscountHasBeenUpdated(LoyaltyProgram loyaltyProgram, string discountId, Money discountPrice) : base(
        $"The {nameof(Discount)} with the ID: [{discountId}] has updated its discount price to {discountPrice.AsLocalFormat()}")
    {
        LoyaltyProgram = loyaltyProgram;
        DiscountId = discountId;
    }

    #endregion
}