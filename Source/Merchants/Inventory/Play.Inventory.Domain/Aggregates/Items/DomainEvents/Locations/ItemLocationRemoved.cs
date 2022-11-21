using Play.Core.Extensions.IEnumerable;
using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record ItemLocationRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public ItemLocationRemoved(Item item, string userId, IEnumerable<string> storeIds) : base(
        $"The {nameof(User)} with the ID: [{userId}] removed the following locations to the Inventory {nameof(Item)} with the ID: [{item.Id}]: Locations: [{storeIds.ToStringAsConcatenatedValues()}]")
    {
        Item = item;
    }

    #endregion
}