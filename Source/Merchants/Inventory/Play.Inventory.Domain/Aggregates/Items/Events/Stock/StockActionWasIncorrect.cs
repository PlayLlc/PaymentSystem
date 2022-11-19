using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain;

public record StockActionWasIncorrect : BrokenRuleOrPolicyDomainEvent<Item, SimpleStringId>
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public StockActionWasIncorrect(Item item, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
    }

    #endregion
}