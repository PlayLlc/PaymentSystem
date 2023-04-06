using Play.Domain.Events;
using Play.Registration.Domain.Entities;

namespace Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents;

public record UserRegistrationPersonalDetailsUpdated : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public UserRegistrationPersonalDetailsUpdated(UserRegistration userRegistration) : base(
        $"The {nameof(UserRegistration)} with ID: [{nameof(userRegistration.GetId)}] has updated their {nameof(PersonalDetail)}")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}