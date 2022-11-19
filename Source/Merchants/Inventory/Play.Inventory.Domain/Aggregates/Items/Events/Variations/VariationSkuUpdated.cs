using Play.Domain.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain;

public record VariationSkuUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly Variation Variation;
    public readonly string UserId;

    #endregion

    #region Constructor

    public VariationSkuUpdated(Item item, Variation variation, string userId, string sku) : base(
        $"The {nameof(Variation)} with the ID: [{variation.GetId()}] has updated its {nameof(Sku)} to: [{sku}];")
    {
        Item = item;
        Variation = variation;
        UserId = userId;
    }

    #endregion
}