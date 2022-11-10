using Microsoft.AspNetCore.Identity;

using Play.Accounts.Api.Services;

namespace Play.Accounts.Api.Extensions
{
    internal static class IdentityResultExtensions
    {
        #region Instance Members

        public static Result ToResult(this IdentityResult result)
        {
            return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
        }

        #endregion
    }
}