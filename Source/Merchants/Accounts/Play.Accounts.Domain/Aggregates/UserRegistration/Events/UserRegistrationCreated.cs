using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationCreated : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public UserRegistrationCreated(UserRegistration userRegistration) : base(
        $"A {nameof(UserRegistration)} has been created with the ID: [{userRegistration.GetId()}]")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}