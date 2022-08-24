using System.Security.Claims;
namespace MerchantPortal.Security.Contracts.Authentication;

public interface ISecurityTokenService
{
    Task<SecurityToken> GenerateAccessTokenAsync(IEnumerable<Claim> claims);

    Task<SecurityToken> GenerateRefreshToken();
}
