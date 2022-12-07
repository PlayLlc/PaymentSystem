using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record RewardsProgramActiveStatusHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Programs Programs;
    public readonly string UserId;
    public bool IsActive;

    #endregion

    #region Constructor

    public RewardsProgramActiveStatusHasBeenUpdated(Programs programs, string userId, bool isActive) : base(
        $"The {nameof(RewardProgram)} has updated its Activation status to: [{isActive}] by the {nameof(User)} with the ID: [{userId}];")
    {
        Programs = programs;
        UserId = userId;
        IsActive = isActive;
    }

    #endregion
}