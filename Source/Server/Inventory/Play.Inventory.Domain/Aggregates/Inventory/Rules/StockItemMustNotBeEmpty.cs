using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public class StockItemMustNotBeEmpty : BusinessRule<Inventory, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;
    private readonly string _StockItemId;
    private readonly IEnumerable<User> _Subscriptions;
    public override string Message => $"Inventory has run out of a {nameof(StockItem)}";

    #endregion

    #region Constructor

    internal StockItemMustNotBeEmpty(Item item, string stockItemId, int quantity)
    {
        _IsValid = item.IsOutOfStockAlertRequired(quantity, out IEnumerable<User>? subscriptions);
        _StockItemId = stockItemId;
        _Subscriptions = subscriptions ?? new List<User>();
    }

    #endregion

    #region Instance Members

    public override NoInventoryAlert CreateBusinessRuleViolationDomainEvent(Inventory inventory) => new(inventory, _StockItemId, _Subscriptions, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}