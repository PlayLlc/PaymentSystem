using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record NoInventoryAlert : BrokenRuleOrPolicyDomainEvent<Item, SimpleStringId>
{
    #region Instance Values

    public readonly Item Item;
    public readonly IEnumerable<User> Subscriptions;

    #endregion

    #region Constructor

    public NoInventoryAlert(Item item, IEnumerable<User> subscriptions, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
        Subscriptions = subscriptions;
    }

    #endregion
}