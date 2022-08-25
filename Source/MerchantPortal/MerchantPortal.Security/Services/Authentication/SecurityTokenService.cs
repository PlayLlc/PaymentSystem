using MerchantPortal.Security.Contracts;
using MerchantPortal.Security.Contracts.Authentication;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace MerchantPortal.Security.Services.Authentication;

public class SecurityTokenService : ISecurityTokenService
{
    private readonly IConfiguration _Configuration;

    public SecurityTokenService(IConfiguration configuration)
    {
        _Configuration = configuration;
    }

    public async Task<SecurityToken> GenerateAccessTokenAsync(IEnumerable<Claim> claims) => throw new NotImplementedException();
    public async Task<SecurityToken> GenerateRefreshToken() => throw new NotImplementedException();
}
