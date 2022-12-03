using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entities;

public class InventoryItem : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _ItemId;
    private readonly MoneyValueObject _Price;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private InventoryItem()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public InventoryItem(VariationDto dto)
    {
        Id = new(dto.Id);
        _ItemId = new(dto.ItemId);
        _Price = dto.Price.AsMoney();
    }

    /// <exception cref="ValueObjectException"></exception>
    public InventoryItem(string id, string itemId, Money price)
    {
        Id = new(id);
        _ItemId = new(itemId);
        _Price = price;
    }

    #endregion

    #region Instance Members

    internal bool IsLowerThanInventoryPrice(Money price) => price < _Price;

    public override SimpleStringId GetId() => Id;

    public override VariationDto AsDto() =>
        new()
        {
            Id = Id,
            ItemId = _ItemId,
            Price = _Price.AsDto()
        };

    #endregion
}