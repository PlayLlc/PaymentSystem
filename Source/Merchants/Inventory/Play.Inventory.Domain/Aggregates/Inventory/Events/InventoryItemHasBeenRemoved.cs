using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record InventoryItemHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly Inventory Inventory;

    #endregion

    #region Constructor

    public InventoryItemHasBeenRemoved(Inventory inventory) : base($"The {nameof(Inventory)} with the ID: [{inventory.GetId()}] has been deleted;")
    {
        Inventory = inventory;
    }

    #endregion
}