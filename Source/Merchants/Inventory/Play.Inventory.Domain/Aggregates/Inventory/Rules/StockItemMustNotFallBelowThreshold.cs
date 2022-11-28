using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public class StockItemMustNotFallBelowThreshold : BusinessRule<Inventory, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;
    private readonly string _StockItemId;
    private readonly IEnumerable<User> _Subscriptions;
    private readonly int _Quantity;
    public override string Message => $"The {nameof(Item)} has fallen below the low inventory threshold";

    #endregion

    #region Constructor

    internal StockItemMustNotFallBelowThreshold(Item item, string stockItemId, int quantity)
    {
        _IsValid = item.IsLowInventoryAlertRequired(quantity, out IEnumerable<User>? subscriptions);
        _StockItemId = stockItemId;
        _Subscriptions = subscriptions ?? new List<User>();
    }

    #endregion

    #region Instance Members

    public override LowInventoryAlert CreateBusinessRuleViolationDomainEvent(Inventory inventory) =>
        new LowInventoryAlert(inventory, _StockItemId, _Subscriptions, _Quantity, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}