using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration
{
    public record MerchantRegistrationCreatedDomainEvent : DomainEvent
    {
        #region Static Metadata

        public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(MerchantRegistrationCreatedDomainEvent));

        #endregion

        #region Instance Values

        public readonly MerchantRegistrationId Id;
        public readonly Name Name;
        public readonly Address Address;
        public readonly BusinessTypes BusinessType;
        public readonly MerchantCategoryCodes MerchantCategoryCode;
        public readonly DateTimeUtc RegisteredDate;
        public RegistrationStatuses Status;

        #endregion

        #region Constructor

        public MerchantRegistrationCreatedDomainEvent(
            MerchantRegistrationId id, Name name, Address address, BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode,
            DateTimeUtc registeredDate, RegistrationStatuses status) : base(DomainEventTypeId)
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