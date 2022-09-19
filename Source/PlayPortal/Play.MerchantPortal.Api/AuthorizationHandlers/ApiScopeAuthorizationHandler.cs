using Microsoft.AspNetCore.Authorization;
using Play.MerchantPortal.Api.Requirements;

namespace Play.MerchantPortal.Api.AuthorizationHandlers;

public class ApiScopeAuthorizationHandler : AuthorizationHandler<ApiScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiScopeRequirement requirement)
    {
        if (!context.User.HasClaim(x => x.Type == "scope"))
            return Task.CompletedTask;

        var scopes = context.User.Claims.Where(x => x.Type == "scope");

        if (scopes.Any(scope => scope.Value == "merchantportal"))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
