using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public record UserRegistrationHasExpired : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRegistrationHasBeenConfirmed));

    #endregion

    #region Instance Values

    public EntityId<string> UserRegistrationId;

    #endregion

    #region Constructor

    public UserRegistrationHasExpired(EntityId<string> userRegistrationId) : base(DomainEventTypeId)
    {
        UserRegistrationId = userRegistrationId;
    }

    #endregion
}