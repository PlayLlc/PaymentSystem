using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record LowInventoryItemThresholdUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LowInventoryItemThresholdUpdated(Item item, string userId, ushort quantity) : base(
        $"The {nameof(User)} with the ID: [{userId}] has updated the {nameof(Alerts)}'s Low Inventory Threshold for the {nameof(Item)} with the ID: [{item.GetId()}] to {quantity};")
    {
        Item = item;
        UserId = userId;
    }

    #endregion
}