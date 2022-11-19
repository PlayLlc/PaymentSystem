using Play.Domain.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

public record SkuUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public SkuUpdated(Item item, Variation variation, string userId, string sku) : base(
        $"The {nameof(Variation)} with the ID: [{variation.GetId()}] has updated its {nameof(Sku)} to: [{sku}];")
    {
        Item = item;
    }

    #endregion
}