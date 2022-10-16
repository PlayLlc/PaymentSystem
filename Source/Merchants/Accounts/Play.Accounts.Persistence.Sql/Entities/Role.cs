using Microsoft.AspNetCore.Identity;

using Play.Accounts.Persistence.Sql.Enums;

namespace Play.Accounts.Persistence.Sql.Entities;

public class Role : IdentityRole
{
    #region Instance Members

    public static IEnumerable<string> GetAllRoles()
    {
        return Enum.GetNames<RoleTypes>().ToList();
    }

    #endregion
}