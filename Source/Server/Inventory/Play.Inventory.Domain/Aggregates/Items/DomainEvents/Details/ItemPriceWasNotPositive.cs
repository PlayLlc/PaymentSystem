using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

/// <summary>
///     An update to the Item's Price was attempted with a zero or negative number
/// </summary>
public record ItemPriceWasNotPositive : BrokenRuleOrPolicyDomainEvent<Item, SimpleStringId>
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public ItemPriceWasNotPositive(Item item, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
    }

    #endregion
}