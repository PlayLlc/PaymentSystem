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
    public uint ConfirmationCode { get; set; }

    #endregion
}