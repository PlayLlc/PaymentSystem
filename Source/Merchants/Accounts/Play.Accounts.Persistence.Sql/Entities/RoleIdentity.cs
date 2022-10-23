using Microsoft.AspNetCore.Identity;

using Play.Accounts.Domain.Enums;

namespace Play.Accounts.Persistence.Sql.Entities;

public sealed class RoleIdentity : IdentityRole
{
    #region Constructor

    public RoleIdentity()
    { }

    public RoleIdentity(string name) : base(name)
    {
        Id = name;
    }

    #endregion

    #region Instance Members

    public static IEnumerable<string> GetAllRoles()
    {
        return UserRoles.Empty.GetAll().Select(a => a.Name);
    }

    #endregion
}