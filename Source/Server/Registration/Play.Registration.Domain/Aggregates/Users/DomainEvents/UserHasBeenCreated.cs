using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record UserHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly User User;

    #endregion

    #region Constructor

    public UserHasBeenCreated(User user) : base($"The {nameof(User)} with the Id: [{user.GetId()}] has been created")
    {
        User = user;
    }

    #endregion
}