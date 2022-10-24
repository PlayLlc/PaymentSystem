using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Api.Models;

public class LoginInputModel
{
    #region Instance Values

    [Required]
    [EmailAddress]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Url]
    public string ReturnUrl { get; set; } = string.Empty;

    public bool RememberLogin { get; set; }

    #endregion
}