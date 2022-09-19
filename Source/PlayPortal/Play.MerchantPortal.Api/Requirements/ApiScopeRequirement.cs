using Microsoft.AspNetCore.Authorization;

namespace Play.MerchantPortal.Api.Requirements;

public class ApiScopeRequirement : IAuthorizationRequirement
{
    public ApiScopeRequirement()
    {

    }
}
