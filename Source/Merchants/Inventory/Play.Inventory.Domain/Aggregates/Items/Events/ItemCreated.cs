using Play.Domain.Events;

namespace Play.Inventory.Domain;

public record ItemCreated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemCreated(Item item, string userId) : base($"A new Inventory {nameof(Item)} with the ID: [{item.GetId()}] has been created")
    {
        Item = item;
        UserId = userId;
    }

    #endregion
}