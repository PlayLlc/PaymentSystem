using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record InventoryCreated : DomainEvent
{
    #region Instance Values

    public readonly Inventory Inventory;

    #endregion

    #region Constructor

    public InventoryCreated(Inventory inventory) : base(
        $"The {nameof(Inventory)} with the ID: [{inventory.GetId()}] was created for the store with ID: [{inventory.GetStoreId()}];")
    {
        Inventory = inventory;
    }

    #endregion
}