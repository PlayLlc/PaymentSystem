using Play.Domain.Events;
using Play.Globalization.Currency;

namespace Play.Inventory.Domain;

public record ItemPriceUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly Money Price;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemPriceUpdated(Item item, string userId, Money price) : base(
        $"The {nameof(Item)} with the ID: [{item.GetId()}] has updated its {nameof(Price)} to: [{price}];")
    {
        Item = item;
        Price = price;
        UserId = userId;
    }

    #endregion
}