using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record VariationCreated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly Variation Variation;
    public readonly string UserId;

    #endregion

    #region Constructor

    public VariationCreated(Item item, Variation variation, string userId) : base(
        $"The {nameof(Item)} with the ID: [{item.GetId()}] created a {nameof(Variation)} with the ID: [{variation.GetId()}];")
    {
        Item = item;
        Variation = variation;
        UserId = userId;
    }

    #endregion
}