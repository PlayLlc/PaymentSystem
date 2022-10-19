using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationCreated : DomainEvent
{
    #region Instance Values

    public readonly string Id;
    public readonly string Email;

    #endregion

    #region Constructor

    public UserRegistrationCreated(string id, string email) : base(
        $"The {nameof(UserRegistration)} with {nameof(Id)}: [{id}]; and {nameof(Email)}: [{email}] has begun the registration process")
    {
        Id = id;
        Email = email;
    }

    #endregion
}