using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Globalization.Currency;

namespace Play.Inventory.Domain;

/// <summary>
///     An update to the Item's Price was attempted with a zero or negative number
/// </summary>
public record ItemPriceWasNotPositive : BrokenRuleOrPolicyDomainEvent<Item, SimpleStringId>
{
    #region Instance Values

    public readonly Item Item;
    public readonly Money Price;

    #endregion

    #region Constructor

    public ItemPriceWasNotPositive(Item item, Money price, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
        Price = price;
    }

    #endregion
}