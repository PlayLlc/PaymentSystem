using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record StockItemCreated : DomainEvent
{
    #region Instance Values

    public readonly Inventory Inventory;
    public readonly StockItem StockItem;

    #endregion

    #region Constructor

    public StockItemCreated(Inventory inventory, StockItem stockItem) : base(
        $"The {nameof(Inventory)} with the ID: [{inventory.GetId()}] created a {nameof(StockItem)} with the ID: [{stockItem.GetId()}];")
    {
        Inventory = inventory;
        StockItem = stockItem;
    }

    #endregion
}