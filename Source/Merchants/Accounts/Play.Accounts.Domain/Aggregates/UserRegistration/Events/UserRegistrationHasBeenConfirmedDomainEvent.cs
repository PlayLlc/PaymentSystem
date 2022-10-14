using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

public record UserRegistrationHasBeenConfirmedDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRegistrationHasBeenConfirmedDomainEvent));

    #endregion

    #region Instance Values

    public string UserRegistrationId;

    #endregion

    #region Constructor

    public UserRegistrationHasBeenConfirmedDomainEvent(string userRegistrationId) : base(DomainEventTypeId)
    {
        UserRegistrationId = userRegistrationId;
    }

    #endregion
}