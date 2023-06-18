using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates.Items.DomainEvents;

public record PriceUpdated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public PriceUpdated(Item item, string userId, Money price) : base(
        $"The {nameof(Inventory)} {nameof(Item)} with the ID: [{item.Id}] has updated its {nameof(price)} to: [{price}] by the user with the {nameof(userId)} {nameof(userId)};")
    {
        Item = item;
    }

    #endregion
}