using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;
using Play.Merchants.Onboarding.Domain.Entities;
using Play.Merchants.Onboarding.Domain.Enums;
using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Common;

public class Address : Entity<string>
{
    #region Instance Values

    public string StreetAddress;
    public string ApartmentNumber;
    public Zipcode Zipcode;
    public StateAbbreviations State;
    public string City;

    public AddressId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    public Address(AddressId id, string streetAddress, string apartmentNumber, string zipcode, StateAbbreviations state, string city)
    {
        Id = id;
        StreetAddress = streetAddress;
        ApartmentNumber = apartmentNumber;
        Zipcode = new Zipcode(zipcode);
        State = state;
        City = city;
    }

    #endregion

    #region Instance Members

    public override AddressId GetId()
    {
        return Id;
    }

    #endregion
}