using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record ItemIsAvailableForAllLocations : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public ItemIsAvailableForAllLocations(Item item, string userId) : base(
        $"The {nameof(User)} with the ID: [{userId}] made the Inventory {nameof(Item)} with the ID: [{item.GetId()}] available to all stores")
    {
        Item = item;
    }

    #endregion
}