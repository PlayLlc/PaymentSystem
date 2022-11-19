using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record ItemVariationRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string VariationItemId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemVariationRemoved(Item item, string variationItemId, string userId) : base(
        $"The {nameof(Item)} with the ID: [{item.GetId()}] removed a variation {nameof(Item)} with the ID: [{variationItemId}];")
    {
        Item = item;
        VariationItemId = variationItemId;
        UserId = userId;
    }

    #endregion
}

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