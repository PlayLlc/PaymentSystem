using Play.Core;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Enums;

namespace Play.Identity.Domain.Entities;

public class UserRole : Entity<SimpleStringId>
{
    #region Instance Values

    public readonly string Name;

    public override SimpleStringId Id { get; }

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

        Id = new SimpleStringId(dto.Name);
        Name = dto.Name;
    }

    public UserRole(UserRoles userRole)
    {
        Id = new SimpleStringId(userRole);
        Name = userRole;
    }

    /// <exception cref="ValueObjectException"></exception>
    public UserRole(string name)
    {
        if (!UserRoles.Empty.TryGet(name, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(UserRole)} with the value: {name} could not be found");

        Id = new SimpleStringId(name);
        Name = name;
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override UserRoleDto AsDto() =>
        new UserRoleDto
        {
            Id = Id,
            Name = Name
        };

    #endregion
}