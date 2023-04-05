using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record UserRolesHaveBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly User User;

    #endregion

    #region Constructor

    public UserRolesHaveBeenUpdated(User user) : base($"The roles for the {nameof(User)} with the ID: [{user.GetId()}] have been updated;")
    {
        User = user;
    }

    #endregion
}