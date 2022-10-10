using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.Users;

public record UserRoleAdded : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRoleAdded));

    #endregion

    #region Instance Values

    public readonly UserId UserId;
    public readonly UserRole UserRole;

    #endregion

    #region Constructor

    public UserRoleAdded(UserId id, UserRole role) : base(DomainEventTypeId)
    {
        UserId = id;
        UserRole = role;
    }

    #endregion
}