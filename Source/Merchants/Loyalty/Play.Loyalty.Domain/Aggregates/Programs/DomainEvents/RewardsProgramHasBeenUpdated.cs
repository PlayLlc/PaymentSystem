using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record RewardsProgramHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Programs Programs;
    public readonly string UserId;

    #endregion

    #region Constructor

    public RewardsProgramHasBeenUpdated(Programs programs, string userId) : base(
        $"The {nameof(RewardProgram)} has been updated for {nameof(Programs)} with the ID: {programs.Id};")
    {
        Programs = programs;
        UserId = userId;
    }

    #endregion
}