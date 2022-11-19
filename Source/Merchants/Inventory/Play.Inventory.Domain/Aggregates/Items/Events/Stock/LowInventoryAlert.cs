using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record LowInventoryAlert : BrokenRuleOrPolicyDomainEvent<Inventory, SimpleStringId>
{
    #region Instance Values

    public readonly Inventory Inventory;
    public readonly string StockItemId;
    public readonly IEnumerable<User> Subscriptions;

    #endregion

    #region Constructor

    public LowInventoryAlert(Inventory inventory, string stockItemId, IEnumerable<User> subscriptions, IBusinessRule rule) : base(inventory, rule)
    {
        Inventory = inventory;
        StockItemId = stockItemId;
        Subscriptions = subscriptions;
    }

    #endregion
}