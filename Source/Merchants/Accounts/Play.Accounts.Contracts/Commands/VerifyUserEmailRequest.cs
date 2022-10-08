using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Contracts.Commands
{
    public class VerifyUserEmailRequest
    {
        #region Instance Values

        /// <summary>
        ///     The user's email
        /// </summary>
        [Required]
        [EmailAddress]
        [MinLength(1)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        ///     The confirmation code sent via email message
        /// </summary>
        [Required]
        [StringLength(6)]
        public string ConfirmationCode { get; set; }

        #endregion
    }
}