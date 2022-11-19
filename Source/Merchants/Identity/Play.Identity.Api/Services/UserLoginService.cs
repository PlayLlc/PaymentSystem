using Duende.IdentityServer;

using Microsoft.AspNetCore.Authentication;

using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Services;
using Play.Core;

namespace Play.Identity.Api.Services;

internal class UserLoginService : ILoginUsers
{
    #region Instance Values

    private readonly IHashPasswords _PasswordHasher;

    #endregion

    #region Constructor

    public UserLoginService(IHashPasswords passwordHasher)
    {
        _PasswordHasher = passwordHasher;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Signs the user in to the authentication framework and optionally creates a cookie to remember the user
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <param name="clearTextPassword"></param>
    /// <returns></returns>
    public async Task<Result> LoginAsync(HttpContext context, User user, string clearTextPassword)
    {
        Result result = user.LoginValidation(_PasswordHasher, clearTextPassword);

        if (!result.Succeeded)
            return result;

        AuthenticationProperties? props = null;
        IdentityServerUser issuer = new IdentityServerUser(user.GetId()) {DisplayName = user.GetEmail()};
        await context.SignInAsync(issuer, props).ConfigureAwait(false);

        return new Result();
    }

    #endregion
}