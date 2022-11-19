using Play.Core.Extensions.IEnumerable;
using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record ItemCategoriesRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;
    public readonly List<string> CategoryIds;

    #endregion

    #region Constructor

    public ItemCategoriesRemoved(Item item, string userId, params Category[] categories) : base(
        $"The {nameof(User)} with the ID: [{userId}] has removed the following Categories: [{categories.Select(a => a.GetName()).ToStringAsConcatenatedValues()}];")
    {
        Item = item;
        UserId = userId;
        CategoryIds = categories.Select(a => a.Id.Value).ToList();
    }

    #endregion
}