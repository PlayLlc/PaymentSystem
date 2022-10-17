using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserCreated : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventType DomainEventType = CreateEventTypeId(typeof(UserCreated));

    #endregion

    #region Instance Values

    public readonly string UserId;

    #endregion

    #region Constructor

    public UserCreated(string id) : base(DomainEventType)
    {
        UserId = id;
    }

    #endregion
}