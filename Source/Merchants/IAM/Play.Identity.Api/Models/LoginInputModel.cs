using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Api.Models;

public class LoginInputModel
{
    #region Instance Values

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }

    #endregion
}