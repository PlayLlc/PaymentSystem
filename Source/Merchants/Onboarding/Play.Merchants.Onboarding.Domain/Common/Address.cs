using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;

namespace Play.Merchants.Onboarding.Domain.Common
{
    public record Address : IEntity<string>
    {
        #region Instance Values

        public readonly string StreetAddress;
        public readonly string ApartmentNumber;
        public readonly string Zipcode;
        public readonly string State;
        public readonly string City;

        public EntityId<string> Id { get; }

        #endregion

        #region Constructor

        public Address(EntityId<string> id, string streetAddress, string apartmentNumber, string zipcode, string state, string city)
        {
            Id = id;
            StreetAddress = streetAddress;
            ApartmentNumber = apartmentNumber;
            Zipcode = zipcode;
            State = state;
            City = city;
        }

        #endregion
    }
}