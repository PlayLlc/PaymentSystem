using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationHasExpired : DomainEvent
{
    #region Instance Values

    public readonly string Id;

    #endregion

    #region Constructor

    public UserRegistrationHasExpired(string id) : base($"The user registration has expired for {nameof(UserRegistration)} with the {nameof(Id)}: [{id}];")
    {
        Id = id;
    }

    #endregion
}