using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Contracts.Commands;

public class ValidateEmailRequest
{
    #region Instance Values

    /// <summary>
    ///     The user's email
    /// </summary>
    [Required]
    [EmailAddress]
    [MinLength(1)]
    public string Email { get; set; } = string.Empty;

    #endregion
}