using Play.Accounts.Domain.Enums;
using Play.Core;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.Entities;

public record UserRole
{
    #region Instance Values

    public readonly string Id;

    public readonly string Value;

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private UserRole()
    { }

    public UserRole(UserRoles userRole)
    {
        Id = userRole;
        Value = userRole;
    }

    public UserRole(string value)
    {
        if (!UserRoles.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(UserRole)} with the value: {value} could not be found");

        Id = value;
        Value = value;
    }

    #endregion
}