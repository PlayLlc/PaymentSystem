﻿using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record ItemRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public ItemRemoved(Item item, string userId) : base($"An Inventory {nameof(Item)} with the ID: [{item.GetId()}] has been removed")
    {
        Item = item;
    }

    #endregion
}