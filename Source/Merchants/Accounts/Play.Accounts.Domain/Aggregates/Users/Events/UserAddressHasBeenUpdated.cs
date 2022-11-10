using Play.Accounts.Domain.Entities;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserAddressHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly User User;

    #endregion

    #region Constructor

    public UserAddressHasBeenUpdated(User user) : base($"The {nameof(User)} with the ID: [{user.GetId()}] has updated its {nameof(Address)};")
    {
        User = user;
    }

    #endregion
}