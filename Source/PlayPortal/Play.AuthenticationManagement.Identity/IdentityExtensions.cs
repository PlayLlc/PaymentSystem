using Microsoft.AspNetCore.Identity;
using Play.AuthenticationManagement.Identity.Services;

namespace Play.AuthenticationManagement.Identity;

internal static class IdentityExtensions
{
    public static RegisterResult ToResult(this IdentityResult result)
    {
        return result.Succeeded
            ? RegisterResult.Success()
            : RegisterResult.Failure(result.Errors.Select(e => e.Description));
    }
}
