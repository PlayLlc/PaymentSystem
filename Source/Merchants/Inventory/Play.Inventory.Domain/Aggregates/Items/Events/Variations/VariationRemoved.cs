using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record VariationRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string VariationId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public VariationRemoved(Item item, string variationId, string userId) : base(
        $"The {nameof(Item)} with the ID: [{item.GetId()}] removed a variation {nameof(Item)} with the ID: [{variationId}];")
    {
        Item = item;
        VariationId = variationId;
        UserId = userId;
    }

    #endregion
}