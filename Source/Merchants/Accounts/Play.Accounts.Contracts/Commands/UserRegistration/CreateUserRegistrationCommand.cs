using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Contracts.Commands
{
    public class CreateUserRegistrationCommand
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
        ///     The user's password
        /// </summary>
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        #endregion
    }
}