using Play.Core.Extensions.IEnumerable;
using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record ItemCategoriesRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public ItemCategoriesRemoved(Item item, string userId, params Category[] categories) : base(
        $"The {nameof(User)} with the ID: [{userId}] has removed the following Categories: [{categories.Select(a => a.GetName()).ToStringAsConcatenatedValues()}];")
    {
        Item = item;
    }

    #endregion
}