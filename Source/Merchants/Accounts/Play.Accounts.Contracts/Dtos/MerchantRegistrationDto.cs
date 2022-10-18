﻿PPlay.Globalization.TimeDtos;

namespace Play.Accounts.Contracts.Dtos
{
    public class MerchantRegistrationDto : IDto
    {
        #region Instance Values

        public string Id { get; set; }
        public string UserRegistrationId { get; set; }
        public string CompanyName { get; set; }
        public AddressDto AddressDto { get; set; }
        public string BusinessType { get; set; }
        public ushort MerchantCategoryCode { get; set; }
        public DateTimeUtc RegisteredDate { get; set; }
        public DateTimeUtc? ConfirmedDate { get; set; }
        public string RegistrationStatus { get; set; }

        #endregion
    }
}