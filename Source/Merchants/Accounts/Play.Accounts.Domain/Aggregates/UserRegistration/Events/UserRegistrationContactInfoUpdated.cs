using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationContactInfoUpdated : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public UserRegistrationContactInfoUpdated(UserRegistration userRegistration) : base(
        $"The {nameof(UserRegistration)} with ID: [{nameof(userRegistration.GetId)}] has updated their contact information")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}