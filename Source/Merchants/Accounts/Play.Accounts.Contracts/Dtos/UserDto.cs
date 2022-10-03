using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Globalization.Time;

namespace Play.Accounts.Contracts.Dtos
{
    public class UserDto : IDto
    {
        #region Instance Values

        public string Id { get; set; } = string.Empty;
        public string MerchantId { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public AddressDto Address { get; set; } = new();
        public ContactInfoDto ContactInfo { get; set; } = new();
        public DateTimeUtc DateOfBirth { get; set; }
        public string LastFourOfSsn { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        #endregion
    }
}