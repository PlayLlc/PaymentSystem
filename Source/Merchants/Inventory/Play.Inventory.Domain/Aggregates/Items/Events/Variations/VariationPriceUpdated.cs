using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record VariationPriceUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly Variation Variation;
    public readonly Money Price;
    public readonly string UserId;

    #endregion

    #region Constructor

    public VariationPriceUpdated(Item item, Variation variation, string userId, Money price) : base(
        $"The {nameof(Variation)} with the ID: [{variation.Id}] has updated its {nameof(Price)} to: [{price.ToString()}];")
    {
        Item = item;
        Variation = variation;
        Price = price;
        UserId = userId;
    }

    #endregion
}