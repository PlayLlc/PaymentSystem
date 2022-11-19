using Play.Domain.Aggregates;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public class ItemStockMustNotFallBeLow : BusinessRule<Item, int>
{
    #region Instance Values

    private readonly bool _IsValid;
    private readonly IEnumerable<User> _Subscriptions;
    public override string Message => $"The {nameof(Item)} has fallen below the low inventory threshold";

    #endregion

    #region Constructor

    internal ItemStockMustNotFallBeLow(Alerts alerts, int quantity)
    {
        _IsValid = alerts.IsLowInventoryAlertRequired(quantity, out IEnumerable<User>? subscriptions);
        _Subscriptions = subscriptions ?? new List<User>();
    }

    #endregion

    #region Instance Members

    public override LowInventoryAlert CreateBusinessRuleViolationDomainEvent(Item item)
    {
        return new LowInventoryAlert(item, _Subscriptions, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}