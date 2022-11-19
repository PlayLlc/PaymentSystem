using Play.Core.Extensions.IEnumerable;
using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record ItemCategoriesAdded : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;
    public readonly List<string> CategoryIds;

    #endregion

    #region Constructor

    public ItemCategoriesAdded(Item item, string userId, params Category[] categories) : base(
        $"The Inventory {nameof(Item)} with ID: [{item.GetId()}] has added the following Category Items [{categories.Select(a => a.GetName()).ToStringAsConcatenatedValues()}];")
    {
        Item = item;
        UserId = userId;
        CategoryIds = categories.Select(a => a.Id.Value).ToList();
    }

    #endregion
}