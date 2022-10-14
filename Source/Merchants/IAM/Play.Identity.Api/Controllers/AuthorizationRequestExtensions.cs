using Duende.IdentityServer.Models;

namespace Play.Identity.Api.Controllers;

public static class AuthorizationRequestExtensions
{
    #region Instance Members

    /// <summary>
    ///     Checks if the redirect URI is for a native client.
    /// </summary>
    /// <returns></returns>
    public static bool IsNativeClient(this AuthorizationRequest context)
    {
        return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal) && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
    }

    #endregion
}