using Play.Accounts.Contracts.Common;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Play.Accounts.Domain.Entities;

public class Address : Entity<string>
{
    #region Instance Values

    public string StreetAddress;
    public string? ApartmentNumber;
    public Zipcode Zipcode;
    public State State;
    public string City;

    public string Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Address(AddressDto dto)
    {
        Id = dto.Id!;

        StreetAddress = dto.StreetAddress;
        ApartmentNumber = dto.ApartmentNumber;
        Zipcode = new Zipcode(dto.Zipcode);
        City = dto.City;
        State = new State(dto.State);
    }

    /// <exception cref="ValueObjectException"></exception>
    public Address(string id, string streetAddress, string zipcode, string stateAbbreviation, string city)
    {
        Id = id;
        StreetAddress = streetAddress;
        Zipcode = new Zipcode(zipcode);
        State = new State(stateAbbreviation);
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
            State = State.Value,
            StreetAddress = StreetAddress,
            Zipcode = Zipcode.Value
        };
    }

    /// <exception cref="NotSupportedException"></exception>
    public string Normalize()
    {
        return JsonSerializer.Serialize(new
        {
            street_address = StreetAddress,
            locality = City,
            region = State,
            postal_code = Zipcode,
            country = "United States"
        });
    }

    #endregion
}