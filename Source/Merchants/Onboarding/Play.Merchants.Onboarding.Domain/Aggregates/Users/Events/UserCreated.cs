using Play.Domain.Events;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public record UserCreated : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserCreated));

    #endregion

    #region Instance Values

    public readonly UserId UserId;

    #endregion

    #region Constructor

    public UserCreated(UserId id) : base(DomainEventTypeId)
    {
        UserId = id;
    }

    #endregion
}