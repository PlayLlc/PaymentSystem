using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyProgram LoyaltyProgram;
    public readonly string DiscountId;

    #endregion

    #region Constructor

    public DiscountHasBeenRemoved(LoyaltyProgram loyaltyProgram, string discountId) : base(
        $"The {nameof(Discount)} with the ID: [{discountId}] has been removed")
    {
        LoyaltyProgram = loyaltyProgram;
        DiscountId = discountId;
    }

    #endregion
}