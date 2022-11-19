using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Entities;

public class Variation : Entity<SimpleStringId>
{
    #region Instance Values

    public Price Price;
    public int Quantity;

    public Name Name;
    public Sku? Sku = null;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Variation()
    { }

    internal Variation(SimpleStringId id, Name name, Price price, int quantity = 0, Sku? sku = null)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
        Sku = sku;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Variation(VariationDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        Name = new Name(dto.Name);
        Price = new Price(dto.Price);
        Quantity = dto.Quantity;
        Sku = string.IsNullOrEmpty(dto.Sku) ? null : new Sku(dto.Sku);
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override VariationDto AsDto()
    {
        return new VariationDto()
        {
            Id = Id,
            Name = Name,
            Price = Price.AsDto(),
            Quantity = Quantity
        };
    }

    #endregion
}