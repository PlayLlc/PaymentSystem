using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record VariationNameUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly Variation Variation;
    public readonly string UserId;

    #endregion

    #region Constructor

    public VariationNameUpdated(Item item, Variation variation, string userId, string name) : base(
        $"The {nameof(Variation)} with the ID: [{variation.Id}] has updated its {nameof(name)} to: [{name}]")
    {
        Item = item;
        Variation = variation;
        UserId = userId;
    }

    #endregion
}