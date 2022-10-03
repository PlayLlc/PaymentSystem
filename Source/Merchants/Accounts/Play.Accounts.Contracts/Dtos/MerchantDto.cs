using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;

namespace Play.Accounts.Contracts.Dtos
{
    public class MerchantDto : IDto
    {
        #region Instance Values

        public string Id { get; set; }
        public string CompanyName { get; set; }
        public AddressDto Address { get; set; }
        public string BusinessType { get; set; }
        public string MerchantCategoryCode { get; set; }

        #endregion
    }
}