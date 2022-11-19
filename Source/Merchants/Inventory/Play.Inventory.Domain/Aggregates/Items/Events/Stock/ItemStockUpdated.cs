using Play.Domain.Events;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain;

public record ItemStockUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;
    public readonly StockAction Action;
    public readonly ushort Quantity;

    #endregion

    #region Constructor

    public ItemStockUpdated(Item item, string userId, StockAction action, ushort quantity) : base(
        $"The Inventory {nameof(Item)} with ID: [{item.GetId()}] has logged a {action.Value} {nameof(StockAction)} with a quantity of {quantity}")
    {
        Item = item;
        UserId = userId;
        Action = action;
        Quantity = quantity;
    }

    #endregion
}