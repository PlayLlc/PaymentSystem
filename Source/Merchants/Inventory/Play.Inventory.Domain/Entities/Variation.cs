using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Entities;

/// <summary>
///     A <see cref="Variation" /> is a distinct type of an <see cref="Item" />. For example, an <see cref="Item" /> can
///     have <see cref="Variation" /> objects for XSmall, Small, Medium to distinguish an <see cref="Item" /> from another
/// </summary>
public class Variation : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly Price _Price;

    /// <summary>
    ///     The name of this <see cref="Variation" />. For example, XSmall, Small, Medium or Red, Blue, Green
    /// </summary>
    private Name _Name;

    private Sku? _Sku = null;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Variation()
    { }

    internal Variation(SimpleStringId id, Name name, Price price, Sku? sku = null)
    {
        Id = id;
        _Name = name;
        _Price = price;
        _Sku = sku;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Variation(VariationDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _Name = new Name(dto.Name);
        _Price = new Price(dto.Price);
        _Sku = string.IsNullOrEmpty(dto.Sku) ? null : new Sku(dto.Sku);
    }

    #endregion

    #region Instance Members

    public string GetName()
    {
        return _Name;
    }

    internal void UpdatePrice(Money price)
    {
        _Price.Amount = price.GetAmount();
    }

    internal void UpdateSku(string sku)
    {
        _Sku = new Sku(sku);
    }

    internal void UpdateName(string name)
    {
        _Name = new Name(name);
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override VariationDto AsDto()
    {
        return new VariationDto()
        {
            Id = Id,
            Name = _Name,
            Price = _Price.AsDto()
        };
    }

    #endregion
}