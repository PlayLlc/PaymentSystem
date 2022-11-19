using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record ItemNameUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public ItemNameUpdated(Item item, string userId, string name) : base(
        $"The Inventory {nameof(Item)} with ID: [{item.GetId()}] has updated its {nameof(Name)}: [{name}];")
    {
        Item = item;
    }

    #endregion
}