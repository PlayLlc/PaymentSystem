using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record NoInventoryAlert : BrokenRuleOrPolicyDomainEvent<Inventory, SimpleStringId>
{
    #region Instance Values

    public readonly Inventory Item;
    public readonly string StockItemId;
    public readonly IEnumerable<User> Subscriptions;

    #endregion

    #region Constructor

    public NoInventoryAlert(Inventory item, string stockItemId, IEnumerable<User> subscriptions, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
        StockItemId = stockItemId;
        Subscriptions = subscriptions;
    }

    #endregion
}