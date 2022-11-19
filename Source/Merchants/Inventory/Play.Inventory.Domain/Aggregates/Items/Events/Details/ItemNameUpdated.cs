using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain;

public record ItemNameUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemNameUpdated(Item item, string userId, string name) : base(
        $"The Inventory {nameof(Item)} with ID: [{item.GetId()}] has updated its {nameof(Name)}: [{name}];")
    {
        Item = item;
        UserId = userId;
    }

    #endregion
}