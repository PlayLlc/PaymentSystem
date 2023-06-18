using Play.Domain.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates.Items.DomainEvents;

public record SkuUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public SkuUpdated(Item item, string userId, string sku) : base(
        $"The {nameof(Inventory)} {nameof(Item)} with the ID: [{item.Id}] has updated its {nameof(Sku)} to: [{sku}] by the {nameof(User)} with the ID: {userId};")
    {
        Item = item;
    }

    #endregion
}