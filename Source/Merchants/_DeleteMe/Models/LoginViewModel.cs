using System.ComponentModel.DataAnnotations;

namespace _DeleteMe.Models;

public class LoginViewModel
{
    #region Instance Values

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string ReturnUrl { get; set; } = string.Empty;

    public bool RememberLogin { get; set; }

    #endregion
}