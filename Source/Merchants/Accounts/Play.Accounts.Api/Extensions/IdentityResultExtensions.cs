using Microsoft.AspNetCore.Identity;

using Play.Accounts.Api.Services;

namespace Play.Accounts.Api.Extensions
{
    internal static class IdentityResultExtensions
    {
        #region Instance Members

        public static RegisterResult ToResult(this IdentityResult result)
        {
            return result.Succeeded ? RegisterResult.Success() : RegisterResult.Failure(result.Errors.Select(e => e.Description));
        }

        #endregion
    }
}