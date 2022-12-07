using Play.Domain.Events;
using Play.Identity.Domain.Entities;

namespace Play.Identity.Domain.Aggregates;

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