using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Contracts.Commands;

public class VerifyUserMobileRequest
{
    #region Instance Values

    [Required]
    public string UserRegistrationId { get; set; }

    /// <summary>
    ///     The identifier of the user registration
    /// </summary>
    [Required]
    [StringLength(6)]
    public string ConfirmationCode { get; set; }

    #endregion
}