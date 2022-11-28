using System.Text.Json;

using Play.Domain.Common.Dtos;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;

namespace Play.Domain.Common.Entities;

public class Address : Entity<SimpleStringId>
{
    #region Instance Values

    public string StreetAddress;
    public string? ApartmentNumber;
    public Zipcode Zipcode;
    public State State;
    public string City;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Address()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Address(AddressDto dto)
    {
        Id = new SimpleStringId(dto.Id!);

        StreetAddress = dto.StreetAddress;
        ApartmentNumber = dto.ApartmentNumber;
        Zipcode = new Zipcode(dto.Zipcode);
        City = dto.City;
        State = new State(dto.State);
    }

    /// <exception cref="ValueObjectException"></exception>
    public Address(string id, string streetAddress, string zipcode, string state, string city)
    {
        Id = new SimpleStringId(id);
        StreetAddress = streetAddress;
        Zipcode = new Zipcode(zipcode);
        State = new State(state);
        City = city;
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override AddressDto AsDto() =>
        new AddressDto
        {
            Id = Id,
            ApartmentNumber = ApartmentNumber,
            City = City,
            State = State.Value,
            StreetAddress = StreetAddress,
            Zipcode = Zipcode.Value
        };

    /// <exception cref="NotSupportedException"></exception>
    public string Normalize() =>
        JsonSerializer.Serialize(new
        {
            street_address = StreetAddress.ToUpper(),
            locality = City.ToUpper(),
            region = State.Value.ToUpper(),
            postal_code = Zipcode.Value.ToUpper(),
            country = "UNITED STATES"
        });

    #endregion
}