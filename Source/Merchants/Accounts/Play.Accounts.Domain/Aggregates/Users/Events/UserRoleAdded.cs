using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.Users;

public record UserRoleAdded : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventType DomainEventType = CreateEventTypeId(typeof(UserRoleAdded));

    #endregion

    #region Instance Values

    public readonly string UserId;
    public readonly UserRole UserRole;

    #endregion

    #region Constructor

    public UserRoleAdded(string id, UserRole role) : base(DomainEventType)
    {
        UserId = id;
        UserRole = role;
    }

    #endregion
}