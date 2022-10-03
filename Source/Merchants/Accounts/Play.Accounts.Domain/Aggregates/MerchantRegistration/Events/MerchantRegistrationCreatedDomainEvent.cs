using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Events;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.Enums;
using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Events
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