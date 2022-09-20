namespace Play.AuthenticationManagement.IdentityServer.Models.Account;

public class LoggedOutViewModel
{
    public string PostLogoutRedirectUri { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;

    public bool AutomaticRedirectAfterSignOut { get; set; }

    public string LogoutId { get; set; } = string.Empty;
}
