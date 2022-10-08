using _DeleteMe.Identity.Enums;

using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations.Schema;

namespace _DeleteMe.Identity.Entities
{
    public class Role : IdentityRole
    {
        #region Instance Members

        public static IEnumerable<string> GetAllRoles()
        {
            return Enum.GetNames<RoleTypes>().ToList();
        }

        #endregion
    }
}