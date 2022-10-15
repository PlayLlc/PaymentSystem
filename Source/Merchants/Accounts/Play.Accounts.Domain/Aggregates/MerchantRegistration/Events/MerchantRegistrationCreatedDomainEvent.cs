using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration
{
    public record MerchantRegistrationCreatedDomainEvent : DomainEvent
    {
        #region Instance Values

        public readonly string Id;
        public readonly Name Name;
        public readonly Address Address;
        public readonly BusinessTypes BusinessType;
        public readonly MerchantCategoryCodes MerchantCategoryCode;
        public readonly DateTimeUtc RegisteredDate;
        public RegistrationStatuses Status;

        #endregion

        #region Constructor

        public MerchantRegistrationCreatedDomainEvent(
            string id, Name name, Address address, BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode, DateTimeUtc registeredDate,
            RegistrationStatuses status) : base($"The {nameof(Merchant)}: [{name}] has begun the registration process")
        {
            Id = id;
            Name = name;
            Address = address;
            BusinessType = businessType;
            MerchantCategoryCode = merchantCategoryCode;
            RegisteredDate = registeredDate;
            Status = status;
        }

        #endregion
    }
}