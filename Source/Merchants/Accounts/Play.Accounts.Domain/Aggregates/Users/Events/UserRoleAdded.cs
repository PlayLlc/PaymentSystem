using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.Users;

public record UserRoleAdded : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRoleAdded));

    #endregion

    #region Instance Values

    public readonly string UserId;
    public readonly UserRole UserRole;

    #endregion

    #region Constructor

    public UserRoleAdded(string id, UserRole role) : base(DomainEventTypeId)
    {
        UserId = id;
        UserRole = role;
    }

    #endregion
}