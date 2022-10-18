using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Contracts.Commands.UserRegistration
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