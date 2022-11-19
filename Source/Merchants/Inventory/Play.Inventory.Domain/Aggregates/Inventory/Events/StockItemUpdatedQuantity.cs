using Play.Domain.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

public record StockItemUpdatedQuantity : DomainEvent
{
    #region Instance Values

    public readonly Inventory Inventory;

    #endregion

    #region Constructor

    public StockItemUpdatedQuantity(Inventory inventory, StockItem stockItem, StockAction action, ushort quantity) : base(
        $"The {nameof(StockItem)} with ID: [{stockItem.GetId()}] has updated its stock. {nameof(StockAction)}: [{action.Value}]; Quantity: [{quantity}]")
    {
        Inventory = inventory;
    }

    #endregion
}