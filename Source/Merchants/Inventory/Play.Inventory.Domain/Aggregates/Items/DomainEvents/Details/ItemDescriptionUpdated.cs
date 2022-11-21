using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record ItemDescriptionUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public ItemDescriptionUpdated(Item item, string userId, string description) : base(
        $"The {nameof(Item)} with the ID: [{item.GetId()}] has updated its description to: [{description}];")
    {
        Item = item;
    }

    #endregion
}