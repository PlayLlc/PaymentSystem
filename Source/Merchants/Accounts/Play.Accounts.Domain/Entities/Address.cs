using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Entities;

namespace Play.Accounts.Domain.Entities;

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
        return (AddressId) Id;
    }

    public override AddressDto AsDto()
    {
        return new AddressDto
        {
            ApartmentNumber = ApartmentNumber,
            City = City,
            StateAbbreviation = State,
            StreetAddress = StreetAddress,
            Zipcode = Zipcode.Value
        };
    }

    #endregion
}