using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Globalization.Time;

namespace Play.Accounts.Contracts.Dtos
{
    public class UserRegistrationDto : IDto
    {
        #region Instance Values

        public string Id { get; set; } = string.Empty;
        public AddressDto Address { get; set; } = new();
        public ContactInfoDto ContactInfo { get; set; } = new();
        public string LastFourOfSsn { get; set; } = string.Empty;
        public DateTimeUtc DateOfBirth { get; set; }
        public DateTimeUtc RegisteredDate { get; set; }
        public DateTimeUtc? ConfirmedDate { get; set; }
        public string RegistrationStatus { get; set; } = string.Empty;

        #endregion
    }
}