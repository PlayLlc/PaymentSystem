using Play.Accounts.Domain.Entities;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

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