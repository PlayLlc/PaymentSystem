using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Events;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;

namespace Play.Merchants.Onboarding.Domain.Users.Events
{
    public record UserRegistrationCreated : DomainEvent
    {
        #region Static Metadata

        public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRegistrationCreated));

        #endregion

        #region Instance Values

        public readonly string StreetAddress;
        public readonly string ApartmentNumber;
        public readonly string Zipcode;
        public readonly string State;
        public readonly string City;
        public readonly string LastFourOfSocialSecurityNumber;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly string Phone;
        public readonly string Email;
        public readonly DateTimeUtc DateOfBirth;
        public readonly DateTimeUtc RegisteredDate;

        #endregion

        #region Constructor

        public UserRegistrationCreated(
            Address address, ContactInfo contactInfo, string lastFourOfSocialSecurityNumber, DateTimeUtc dateOfBirth, DateTimeUtc registeredDate) : base(
            DomainEventTypeId)
        {
            StreetAddress = address.StreetAddress;
            ApartmentNumber = address.ApartmentNumber;
            Zipcode = address.Zipcode;
            State = address.State;
            City = address.City;

            LastFourOfSocialSecurityNumber = lastFourOfSocialSecurityNumber;

            FirstName = contactInfo.FirstName;
            LastName = contactInfo.LastName;
            Phone = contactInfo.Phone;
            Email = contactInfo.Email;
            DateOfBirth = dateOfBirth;
            RegisteredDate = registeredDate;
        }

        #endregion
    }
}