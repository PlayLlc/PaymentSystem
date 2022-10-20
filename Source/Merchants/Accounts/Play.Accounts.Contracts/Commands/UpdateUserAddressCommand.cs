using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands
{
    public record UpdateUserAddressCommand
    {
        #region Instance Values

        [Required]
        public AddressDto Address { get; set; } = new();

        #endregion
    }

    public record UpdateUserContactCommand
    {
        #region Instance Values

        [Required]
        public ContactDto Contact { get; set; } = new();

        #endregion
    }
}