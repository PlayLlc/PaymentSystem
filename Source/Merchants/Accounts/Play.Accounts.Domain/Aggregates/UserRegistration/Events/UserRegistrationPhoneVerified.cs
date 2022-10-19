using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationPhoneVerified : DomainEvent
{
    #region Instance Values

    public readonly string Id;
    public readonly string Username;

    #endregion

    #region Constructor

    public UserRegistrationPhoneVerified(string id) : base(
        $"The {nameof(UserRegistration)} with {nameof(Id)}: [{id}] has successfully verified their mobile device")
    {
        Id = id;
    }

    #endregion
}