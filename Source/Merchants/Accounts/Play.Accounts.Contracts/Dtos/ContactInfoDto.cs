using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;

namespace Play.Accounts.Contracts.Dtos
{
    public class ContactInfoDto : IDto
    {
        #region Instance Values

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        #endregion
    }
}