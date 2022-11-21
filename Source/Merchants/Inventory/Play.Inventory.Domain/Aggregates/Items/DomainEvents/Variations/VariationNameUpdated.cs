using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record VariationNameUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public VariationNameUpdated(Item item, Variation variation, string userId, string name) : base(
        $"The {nameof(Variation)} with the ID: [{variation.Id}] has updated its {nameof(name)} to: [{name}]")
    {
        Item = item;
    }

    #endregion
}