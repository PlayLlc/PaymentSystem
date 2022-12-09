using Play.Domain.Events;
using Play.Loyalty.Domain.Aggregates._External;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountProgramActiveStatusHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Programs Programs;
    public readonly string UserId;
    public bool IsActive;

    #endregion

    #region Constructor

    public DiscountProgramActiveStatusHasBeenUpdated(Programs programs, string userId, bool isActive) : base(
        $"The {nameof(DiscountProgram)} has updated its Activation status to: [{isActive}] by the {nameof(User)} with the ID: [{userId}];")
    {
        Programs = programs;
        UserId = userId;
        IsActive = isActive;
    }

    #endregion
}