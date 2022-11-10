using Play.Accounts.Contracts.Commands;

namespace Play.Accounts.Api.Services;

public interface IIdentityService
{
    #region Instance Members

    Task SignOutAsync();

    Task<SignInResponse> SignInUserAsync(string username, string password, bool rememberLogin);

    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest value);
    Task<bool> ValidateUsername(ValidateEmailRequest request);

    #endregion
}