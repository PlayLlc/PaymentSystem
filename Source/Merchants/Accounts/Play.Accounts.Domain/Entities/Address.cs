using Play.Accounts.Contracts.Common;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Domain.ValueObjectsd;

namespace Play.Accounts.Domain.Entities;

public class Address : Entity<string>
{
    #region Instance Values

    public string StreetAddress;
    public string? ApartmentNumber;
    public Zipcode Zipcode;
    public StateAbbreviation StateAbbreviation;
    public string City;

    public string Id { get; }

    #endregion

    #region Constructor

    public Address(AddressDto dto)
    {
        Id = dto.Id!;

        StreetAddress = dto.StreetAddress;
        ApartmentNumber = dto.ApartmentNumber;
        Zipcode = new Zipcode(dto.Zipcode);
        City = dto.City;
        StateAbbreviation = new StateAbbreviation(dto.StateAbbreviation);
    }

    /// <exception cref="ValueObjectException"></exception>
    public Address(string id, string streetAddress, string zipcode, string stateAbbreviation, string city)
    {
        Id = id;
        StreetAddress = streetAddress;
        Zipcode = new Zipcode(zipcode);
        StateAbbreviation = new StateAbbreviation(stateAbbreviation);
        City = city;
    }

    #endregion

    #region Instance Members

    public override string GetId()
    {
        return Id;
    }

    public override AddressDto AsDto()
    {
        return new AddressDto
        {
            ApartmentNumber = ApartmentNumber,
            City = City,
            StateAbbreviation = StateAbbreviation.Value,
            StreetAddress = StreetAddress,
            Zipcode = Zipcode.Value
        };
    }

    #endregion
}