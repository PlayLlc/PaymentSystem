using System.Security.Claims;

using Duende.IdentityServer.Test;

using IdentityModel;

namespace Play.Accounts.Api;

internal class Users
{
    #region Instance Members

    public static List<TestUser> Get()
    {
        return new List<TestUser>
        {
            new()
            {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE", Username = "scott", Password = "password",
                Claims = new List<Claim> {new(JwtClaimTypes.Email, "scott@scottbrady91.com"), new(JwtClaimTypes.Role, "admin")}
            }
        };
    }

    #endregion
}