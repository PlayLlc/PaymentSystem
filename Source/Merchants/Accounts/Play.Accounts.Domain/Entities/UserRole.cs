using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Enums;
using Play.Core;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.Entities;

public class UserRole : Entity<string>
{
    #region Instance Values

    public readonly string Id;

    public readonly string Name;

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private UserRole()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public UserRole(UserRoleDto dto)
    {
        if (!UserRoles.Empty.TryGet(dto.Name, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(UserRole)} with the value: {dto.Name} could not be found");

        Id = dto.Name;
        Name = dto.Name;
    }

    public UserRole(UserRoles userRole)
    {
        Id = userRole;
        Name = userRole;
    }

    /// <exception cref="ValueObjectException"></exception>
    public UserRole(string name)
    {
        if (!UserRoles.Empty.TryGet(name, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(UserRole)} with the value: {name} could not be found");

        Id = name;
        Name = name;
    }

    #endregion

    #region Instance Members

    public override string GetId()
    {
        return Id;
    }

    public override UserRoleDto AsDto()
    {
        return new UserRoleDto
        {
            Id = Id,
            Name = Name
        };
    }

    #endregion
}