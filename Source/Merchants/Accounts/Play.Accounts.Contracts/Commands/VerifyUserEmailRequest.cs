using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Contracts.Commands
{
    public class VerifyUserEmailRequest
    {
        #region Instance Values

        /// <summary>
        ///     The identifier of the user registration
        /// </summary>
        [Required]
        public string UserRegistrationId { get; set; }

        /// <summary>
        ///     The confirmation code sent via email message
        /// </summary>
        [Required]
        [StringLength(6)]
        public string ConfirmationCode { get; set; }

        #endregion
    }
}