using MerchantPortal.Security.Contracts;
using MerchantPortal.Security.Contracts.Authentication;

namespace MerchantPortal.Security.Services.Authentication;

public class IdentityService : IIdentityService
{
    public async Task<RegistrationResult> RegisterUserAsync(UserDetails userDetails) => throw new NotImplementedException();
    public async Task<AuthenticationResult> ValidateAuthenticationAsync(string username, string password) => throw new NotImplementedException();
}
