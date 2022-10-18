using Play.Accounts.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain;

public record UserRegistrationContactInfoUpdated : DomainEvent
{
    #region Instance Values

    public readonly string Id;
    public readonly string Email;

    #endregion

    #region Constructor

    public UserRegistrationContactInfoUpdated(string id) : base(
        $"The {nameof(UserRegistration)} with {nameof(Id)}: [{id}] has updated their contact information")
    {
        Id = id;
    }

    #endregion
}