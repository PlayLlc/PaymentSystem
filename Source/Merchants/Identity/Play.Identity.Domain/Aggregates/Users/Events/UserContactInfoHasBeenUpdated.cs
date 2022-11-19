using Play.Domain.Common.Entities;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates.Events;

public record UserContactInfoHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly User User;

    #endregion

    #region Constructor

    public UserContactInfoHasBeenUpdated(User user) : base($"The {nameof(User)} with the ID: [{user.GetId()}] has updated its {nameof(Contact)} info;")
    {
        User = user;
    }

    #endregion
}