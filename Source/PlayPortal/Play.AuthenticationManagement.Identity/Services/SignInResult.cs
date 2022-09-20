using Play.AuthenticationManagement.Identity.Models;

namespace Play.AuthenticationManagement.Identity.Services;

public class SignInResult
{
    public bool Succeeded { get; set; }

    public bool ChangePassword { get; set; }

    public User User { get; set; } = default!;

    public static SignInResult Success(User user)
    {
        return new SignInResult
        {
            Succeeded = true,
            User = user
        };
    }

    public static SignInResult ChangePasswordRequired(User user)
    {
        return new SignInResult
        {
            ChangePassword = true,
            User = user
        };
    }

    public static SignInResult FailedSignIn()
    {
        return new SignInResult
        {
            Succeeded = false
        };
    }
}
