using System.ComponentModel.DataAnnotations;

using Play.Mvc.Attributes;

namespace Play.Registration.Api.Models;

public class LoginViewModel
{
    #region Instance Values

    public IEnumerable<ExternalProviderModel> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProviderModel>();

    [Required]
    [EmailAddress]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Http]
    public string ReturnUrl { get; set; } = string.Empty;

    #endregion
}