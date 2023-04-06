using Play.Domain.Events;

namespace Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents;

public record UserRegistrationPhoneVerified : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public UserRegistrationPhoneVerified(UserRegistration userRegistration) : base(
        $"The {nameof(UserRegistration)} with ID: [{userRegistration.GetId()}] has successfully verified their mobile device")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}