using Play.Domain.Events;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain;

public record ItemSkuUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemSkuUpdated(Item item, string userId, Sku sku) : base(
        $"The {nameof(Item)} with the ID: [{item.GetId()}] has updated its {nameof(Sku)} to: [{sku}];")
    {
        Item = item;
        UserId = userId;
    }

    #endregion
}