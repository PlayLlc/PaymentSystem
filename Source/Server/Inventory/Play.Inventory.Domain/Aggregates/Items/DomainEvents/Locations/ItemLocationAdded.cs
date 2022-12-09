﻿using Play.Core.Extensions.IEnumerable;
using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record ItemLocationAdded : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public IEnumerable<string> StoreIds;
    public IEnumerable<string> VariationIds;

    #endregion

    #region Constructor

    public ItemLocationAdded(Item item, string userId, IEnumerable<string> storeIds, IEnumerable<string> variationIds) : base(
        $"The {nameof(User)} with the ID: [{userId}] added the following locations to the Inventory {nameof(Item)} with the ID: [{item.Id}]: Locations: [{storeIds.ToStringAsConcatenatedValues()}]")
    {
        Item = item;
        StoreIds = storeIds;
        VariationIds = variationIds;
    }

    #endregion
}