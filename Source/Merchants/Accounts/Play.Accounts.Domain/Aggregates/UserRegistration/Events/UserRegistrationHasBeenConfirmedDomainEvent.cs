using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public record UserRegistrationHasBeenConfirmedDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRegistrationHasBeenConfirmedDomainEvent));

    #endregion

    #region Instance Values

    public EntityId<string> UserRegistrationId;

    #endregion

    #region Constructor

    public UserRegistrationHasBeenConfirmedDomainEvent(EntityId<string> userRegistrationId) : base(DomainEventTypeId)
    {
        UserRegistrationId = userRegistrationId;
    }

    #endregion
}