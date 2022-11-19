using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record CategoryHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly Category Category;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public CategoryHasBeenRemoved(Category category, string merchantId, string userId) : base(
        $"The {nameof(Category)}: [{category.GetName()}] has been removed for the {nameof(Merchant)} with the ID: [{merchantId}];")
    {
        Category = category;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}