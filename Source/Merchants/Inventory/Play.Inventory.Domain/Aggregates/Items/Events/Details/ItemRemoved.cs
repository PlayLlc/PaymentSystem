using Play.Domain.Events;

namespace Play.Inventory.Domain;

public record ItemRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemRemoved(Item item, string userId) : base($"An Inventory {nameof(Item)} with the ID: [{item.GetId()}] has been removed")
    {
        Item = item;
        UserId = userId;
    }

    #endregion
}