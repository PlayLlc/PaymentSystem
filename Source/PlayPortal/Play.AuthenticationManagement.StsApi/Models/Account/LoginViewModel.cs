using System.ComponentModel.DataAnnotations;

namespace Play.AuthenticationManagement.IdentityServer.Models.Account;

public class LoginViewModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string ReturnUrl { get; set; } = string.Empty;

    public bool RememberLogin { get; set; } = true;
}
