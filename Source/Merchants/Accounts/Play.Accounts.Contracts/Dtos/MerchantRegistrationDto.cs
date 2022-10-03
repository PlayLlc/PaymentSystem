using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Globalization.Time;

namespace Play.Accounts.Contracts.Dtos
{
    public class MerchantRegistrationDto : IDto
    {
        #region Instance Values

        public string Id { get; set; }
        public string UserRegistrationId { get; set; }
        public string CompanyName { get; set; }
        public AddressDto Address { get; set; }
        public string BusinessType { get; set; }
        public string MerchantCategoryCode { get; set; }
        public DateTimeUtc RegisteredDate { get; set; }
        public DateTimeUtc? ConfirmedDate { get; set; }
        public string RegistrationStatus { get; set; }

        #endregion
    }
}