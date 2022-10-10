using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

public record UserRegistrationHasExpiredDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRegistrationHasBeenConfirmedDomainEvent));

    #endregion

    #region Instance Values

    public EntityId<string> UserRegistrationId;

    #endregion

    #region Constructor

    public UserRegistrationHasExpiredDomainEvent(EntityId<string> userRegistrationId) : base(DomainEventTypeId)
    {
        UserRegistrationId = userRegistrationId;
    }

    #endregion
}