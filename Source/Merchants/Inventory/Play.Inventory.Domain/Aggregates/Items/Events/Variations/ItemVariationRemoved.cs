using Play.Domain.Events;

namespace Play.Inventory.Domain;

public record ItemVariationRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string VariationItemId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemVariationRemoved(Item item, string variationItemId, string userId) : base(
        $"The {nameof(Item)} with the ID: [{item.GetId()}] removed a variation {nameof(Item)} with the ID: [{variationItemId}];")
    {
        Item = item;
        VariationItemId = variationItemId;
        UserId = userId;
    }

    #endregion
}