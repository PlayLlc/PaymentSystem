namespace MerchantPortal.Security.Contracts.Authentication;

public interface IIdentityService
{
    Task<AuthenticationResult> ValidateAuthenticationAsync(string username, string password);

    Task<RegistrationResult> RegisterUserAsync(UserDetails userDetails);
}
