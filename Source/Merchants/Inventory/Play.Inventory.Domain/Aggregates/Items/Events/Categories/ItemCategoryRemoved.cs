using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record ItemCategoryRemoved : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;
    public readonly string CategoryId;

    #endregion

    #region Constructor

    public ItemCategoryRemoved(Item item, string userId, string categoryId) : base(
        $"The {nameof(User)} with the ID: [{userId}] has removed the {nameof(Category)} with the ID: [{categoryId}] for the {nameof(Item)} with the ID: [{item.GetId()}];")
    {
        Item = item;
        UserId = userId;
        CategoryId = categoryId;
    }

    #endregion
}