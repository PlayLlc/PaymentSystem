using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Contracts.Commands.UserRegistration;

public class CreateUserRegistrationCommand
{
    #region Instance Values

    /// <summary>
    ///     The user's email
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     The user's password
    /// </summary>
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    #endregion
}