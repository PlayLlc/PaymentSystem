using Play.Domain.Events;

namespace Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents;

public record UserRegistrationHasBeenApproved : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public UserRegistrationHasBeenApproved(UserRegistration userRegistration) : base(
        $"The {nameof(UserRegistration)} with ID: [{userRegistration.GetId()}] has been approved")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}