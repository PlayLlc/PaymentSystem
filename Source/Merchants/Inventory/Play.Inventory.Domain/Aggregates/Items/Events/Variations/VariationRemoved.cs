using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record VariationRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public VariationRemoved(Item item, string variationItemId, string userId) : base(
        $"The {nameof(Item)} with the ID: [{item.GetId()}] removed a variation {nameof(Item)} with the ID: [{variationItemId}];")
    {
        Item = item;
    }

    #endregion
}