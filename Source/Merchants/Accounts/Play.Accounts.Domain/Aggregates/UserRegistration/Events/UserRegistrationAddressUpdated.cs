using Play.Accounts.Domain.Entities;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationAddressUpdated : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public UserRegistrationAddressUpdated(UserRegistration userRegistration) : base(
        $"The {nameof(UserRegistration)} with ID: [{nameof(userRegistration.GetId)}] has updated their {nameof(Address)}")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}