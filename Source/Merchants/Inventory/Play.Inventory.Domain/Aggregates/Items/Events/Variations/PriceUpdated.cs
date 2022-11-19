using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record PriceUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public PriceUpdated(Item item, Variation variation, string userId, Money price) : base(
        $"The {nameof(Variation)} with the ID: [{variation.Id}] has updated its {nameof(Price)} to: [{price.ToString()}];")
    {
        Item = item;
    }

    #endregion
}