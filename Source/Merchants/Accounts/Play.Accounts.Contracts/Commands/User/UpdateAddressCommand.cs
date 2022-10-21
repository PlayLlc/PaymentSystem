using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands.User
{
    public record UpdateAddressCommand
    {
        #region Instance Values

        [Required]
        [MinLength(1)]
        public string Id { get; set; } = string.Empty;

        [Required]
        public AddressDto Address { get; set; } = new();

        #endregion
    }
}