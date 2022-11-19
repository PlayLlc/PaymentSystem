using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain;

public record CategoryMerchantDidNotMatch : BrokenRuleOrPolicyDomainEvent<Item, SimpleStringId>
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public CategoryMerchantDidNotMatch(Item item, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
    }

    #endregion
}