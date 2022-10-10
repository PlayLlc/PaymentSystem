namespace Play.Accounts.Api.Services;

public class SignInResult
{
    #region Instance Values

    public bool Succeeded { get; set; }

    public bool ChangePassword { get; set; }

    public ApplicationUser? IdentityUser { get; set; } = null;

    #endregion

    #region Instance Members

    public static SignInResult Success(ApplicationUser applicationUser)
    {
        return new SignInResult
        {
            Succeeded = true,
            IdentityUser = applicationUser
        };
    }

    public static SignInResult ChangePasswordRequired(ApplicationUser applicationUser)
    {
        return new SignInResult
        {
            ChangePassword = true,
            IdentityUser = applicationUser
        };
    }

    public static SignInResult FailedSignIn()
    {
        return new SignInResult {Succeeded = false};
    }

    #endregion
}