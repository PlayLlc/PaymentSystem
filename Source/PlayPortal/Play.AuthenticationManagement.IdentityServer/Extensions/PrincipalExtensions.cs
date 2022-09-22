using IdentityModel;
using System.Security.Claims;
using System.Security.Principal;

namespace Play.AuthenticationManagement.IdentityServer.Extensions;

public static class PrincipalExtensions
{
    public static string GetSubjectId(this IPrincipal principal)
    {
        return (principal.Identity as ClaimsIdentity)?.FindFirst(JwtClaimTypes.Subject)?.Value ?? throw new InvalidOperationException("Sub claim is missing !");
    }

    public static string GetDisplayName(this IPrincipal principal)
    {
        return principal.Identity?.Name ?? string.Empty;
    }
}
