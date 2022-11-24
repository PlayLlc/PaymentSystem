using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly Programs Programs;
    public readonly string DiscountId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public DiscountHasBeenRemoved(Programs programs, string discountId, string userId) : base(
        $"The {nameof(Discount)} with the ID: [{discountId}] has been removed")
    {
        Programs = programs;
        DiscountId = discountId;
        UserId = userId;
    }

    #endregion
}