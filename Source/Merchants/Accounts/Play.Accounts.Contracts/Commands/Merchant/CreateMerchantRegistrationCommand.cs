using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands.Merchant
{
    public record CreateMerchantRegistrationCommand
    {
        #region Instance Values

        [Required]
        public UserDto User { get; set; } = new();

        [Required]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        #endregion
    }
}