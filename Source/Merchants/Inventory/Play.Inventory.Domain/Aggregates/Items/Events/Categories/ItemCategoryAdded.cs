using Play.Domain.Events;

namespace Play.Inventory.Domain;

public record ItemCategoryAdded : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;
    public readonly string CategoryId;

    #endregion

    #region Constructor

    public ItemCategoryAdded(Item item, Category category, string userId) : base(
        $"The Inventory {nameof(Item)} with ID: [{item.GetId()}] has added the {nameof(Category)}: [{category.GetName()}] with the ID: [{category.GetId()}];")
    {
        Item = item;
        UserId = userId;
        CategoryId = category.Id;
    }

    #endregion
}