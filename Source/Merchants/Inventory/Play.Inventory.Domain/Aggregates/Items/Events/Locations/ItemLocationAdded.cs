using Play.Core.Extensions.IEnumerable;
using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record ItemLocationAdded : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;
    public readonly IEnumerable<string> StoreIds;

    #endregion

    #region Constructor

    public ItemLocationAdded(Item item, string userId, IEnumerable<string> storeIds) : base(
        $"The {nameof(User)} with the ID: [{userId}] added the following locations to the Inventory {nameof(Item)} with the ID: [{item.Id}]: Locations: [{storeIds.ToStringAsConcatenatedValues()}]")
    {
        Item = item;
        StoreIds = storeIds;
        UserId = userId;
    }

    #endregion
}