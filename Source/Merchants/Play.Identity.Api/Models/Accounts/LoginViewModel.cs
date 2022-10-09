using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Api.Models.Accounts;

public class LoginViewModel
{
    #region Instance Values

    public bool AllowRememberLogin { get; set; } = true;

    public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }

    #endregion
}