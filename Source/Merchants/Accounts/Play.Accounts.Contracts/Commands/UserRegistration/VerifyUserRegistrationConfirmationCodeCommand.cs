using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Contracts.Commands;

public class VerifyUserRegistrationConfirmationCodeCommand
{
    #region Instance Values

    public string UserRegistrationId { get; set; } = string.Empty;

    /// <summary>
    ///     The confirmation code sent via email message
    /// </summary>
    [Required]
    public uint ConfirmationCode { get; set; }

    #endregion
}