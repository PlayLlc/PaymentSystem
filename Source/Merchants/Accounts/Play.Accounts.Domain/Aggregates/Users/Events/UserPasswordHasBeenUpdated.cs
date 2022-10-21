using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserPasswordHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly User User;

    #endregion

    #region Constructor

    public UserPasswordHasBeenUpdated(User user) : base($"The {nameof(User)} with the ID: [{user.GetId()}] has updated its hashed password;")
    {
        User = user;
    }

    #endregion
}