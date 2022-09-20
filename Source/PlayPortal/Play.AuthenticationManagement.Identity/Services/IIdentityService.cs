namespace Play.AuthenticationManagement.Identity.Services;

public interface IIdentityService
{
    Task SignOutAsync();

    Task<SignInResult> SignInUserAsync(string username, string password, bool rememberLogin);

    Task<RegisterResult> RegisterUserAsync(CreateUserInput input);
}
