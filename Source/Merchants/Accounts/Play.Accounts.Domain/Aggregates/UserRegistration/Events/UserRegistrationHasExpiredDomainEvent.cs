using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

public record UserRegistrationHasExpiredDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventType DomainEventType = CreateEventTypeId(typeof(UserRegistrationHasBeenConfirmedDomainEvent));

    #endregion

    #region Instance Values

    public string UserRegistrationId;

    #endregion

    #region Constructor

    public UserRegistrationHasExpiredDomainEvent(string userRegistrationId) : base(DomainEventType)
    {
        UserRegistrationId = userRegistrationId;
    }

    #endregion
}