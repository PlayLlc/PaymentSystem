using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record StockItemHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly Inventory Inventory;

    #endregion

    #region Constructor

    public StockItemHasBeenRemoved(Inventory inventory, string stockItemId) : base(
        $"The {nameof(Item)} with the ID: [{inventory.GetId()}] removed a {nameof(StockItem)} with the ID: [{stockItemId}];")
    {
        Inventory = inventory;
    }

    #endregion
}