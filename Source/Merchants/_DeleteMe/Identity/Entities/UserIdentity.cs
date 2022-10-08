using Microsoft.AspNetCore.Identity;

namespace _DeleteMe.Identity.Entities
{
    public class UserIdentity : IdentityUser
    {
        #region Instance Values

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        #endregion
    }
}