using Microsoft.AspNetCore.Identity;

using Play.Identity.Api.Identity.Entities;

namespace Play.Identity.Api.Extensions;

public static class UserManagerExtensions
{
    #region Instance Members

    internal static async Task<Result> ValidatePasswordPolicies(this UserManager<UserIdentity> userManager, string password)
    {
        foreach (IPasswordValidator<UserIdentity> validator in userManager.PasswordValidators)
        {
            IdentityResult? validationResult = await validator.ValidateAsync(userManager, new UserIdentity(), password).ConfigureAwait(false);

            if (!validationResult.Succeeded)
                return new Result(validationResult.Errors.Select(a => a.Description));
        }

        return new Result();
    }

    #endregion
}