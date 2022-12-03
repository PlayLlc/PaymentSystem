using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Domain.Entities;

/// <summary>
///     A <see cref="StockItem" /> is the record of the <see cref="Item" /> quantity that a given Store currently has
/// </summary>
public class StockItem : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _ItemId;
    private readonly SimpleStringId _VariationId;
    private int _Quantity;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public StockItem(StockItemDto dto)
    {
        Id = new(dto.Id);
    }

    /// <exception cref="ValueObjectException"></exception>
    public StockItem(string id, string itemId, string variationId, int quantity)
    {
        Id = new(id);
        _ItemId = new(itemId);
        _VariationId = new(variationId);
        _Quantity = quantity;
    }

    // Constructor for Entity Framework
    private StockItem()
    { }

    #endregion

    #region Instance Members

    internal string GetVariationId() => _VariationId;

    public string GetItemId() => _ItemId;

    internal void AddQuantity(ushort quantity)
    {
        _Quantity += quantity;
    }

    internal void RemoveQuantity(ushort quantity)
    {
        _Quantity -= quantity;
    }

    public override SimpleStringId GetId() => Id;

    public int GetQuantity() => _Quantity;

    public override StockItemDto AsDto() =>
        new()
        {
            Id = Id,
            ItemId = _ItemId,
            VariationId = _VariationId,
            Quantity = _Quantity
        };

    #endregion
}