using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Play.AuthenticationManagement.IdentityServer.Models.Account;

public class RegisterViewModel
{
    [Required]
    [DisplayName("First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [DisplayName("Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [DisplayName("Email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [DisplayName("Username")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DisplayName("Use email address as username")]
    public bool UseEmailAsUserName { get; set; }

    [Required]
    public string ReturnUrl { get; set; } = string.Empty;
}
