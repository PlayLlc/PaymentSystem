using Play.Domain.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain;

public record VariationStockUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly Variation Variation;
    public readonly string UserId;
    public readonly StockAction Action;
    public readonly ushort Quantity;

    #endregion

    #region Constructor

    public VariationStockUpdated(Item item, Variation variation, string userId, StockAction action, ushort quantity) : base(
        $"The {nameof(Variation)} with ID: [{variation.GetId()}] has updated its stock. {nameof(StockAction)}: [{action.Value}]; Quantity: [{quantity}]")
    {
        Item = item;
        Variation = variation;
        UserId = userId;
        Action = action;
        Quantity = quantity;
    }

    #endregion
}