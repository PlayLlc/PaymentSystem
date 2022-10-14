namespace Play.Identity.Api.Models;

public class AccountOptions
{
    #region Static Metadata

    public static bool AllowLocalLogin = true;
    public static bool AllowRememberLogin = true;
    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    public static bool ShowLogoutPrompt = true;
    public static bool AutomaticRedirectAfterSignOut = false;

    public static string InvalidCredentialsErrorMessage = "Invalid username or password";

    #endregion
}