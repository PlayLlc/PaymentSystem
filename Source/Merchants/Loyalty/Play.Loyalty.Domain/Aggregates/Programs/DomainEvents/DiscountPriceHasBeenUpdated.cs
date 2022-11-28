using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountPriceHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Programs Programs;
    public readonly Money Price;
    public readonly string DiscountId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public DiscountPriceHasBeenUpdated(Programs programs, string discountId, string userId, Money price) : base(
        $"The {nameof(Discount)} with the ID: [{discountId}] has updated its discount price to {price.AsLocalFormat()}")
    {
        Programs = programs;
        DiscountId = discountId;
        Price = price;
        UserId = userId;
    }

    #endregion
}