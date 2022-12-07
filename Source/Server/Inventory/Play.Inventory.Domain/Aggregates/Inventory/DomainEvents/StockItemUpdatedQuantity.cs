using Play.Domain.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

public record StockItemUpdatedQuantity : DomainEvent
{
    #region Instance Values

    public readonly Inventory Inventory;
    public readonly string StockId;
    public readonly StockAction StockAction;
    public readonly ushort QuantityUpdated;
    public readonly int TotalQuantity;

    #endregion

    #region Constructor

    public StockItemUpdatedQuantity(Inventory inventory, string stockId, StockAction action, ushort quantityUpdated, int totalQuantity) : base(
        $"The {nameof(StockAction)} {action} was implemented on {nameof(StockItem)} with ID: [{stockId}]. {totalQuantity} items were updated;")
    {
        Inventory = inventory;
        StockId = stockId;
        StockAction = action;
        QuantityUpdated = quantityUpdated;
        TotalQuantity = totalQuantity;
    }

    #endregion
}