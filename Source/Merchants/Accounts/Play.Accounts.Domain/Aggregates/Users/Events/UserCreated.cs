using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.Users;

public record UserCreated : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserCreated));

    #endregion

    #region Instance Values

    public readonly string UserId;

    #endregion

    #region Constructor

    public UserCreated(string id) : base(DomainEventTypeId)
    {
        UserId = id;
    }

    #endregion
}