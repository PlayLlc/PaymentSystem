using Play.Domain.Aggregates;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public class ItemStockMustNotBeEmpty : BusinessRule<Item, int>
{
    #region Instance Values

    private readonly bool _IsValid;
    private readonly IEnumerable<User> _Subscriptions;
    public override string Message => $"There is not enough items in inventory to perform the requested action";

    #endregion

    #region Constructor

    internal ItemStockMustNotBeEmpty(Alerts alerts, int quantity)
    {
        _IsValid = alerts.IsOutOfStockAlertRequired(quantity, out IEnumerable<User>? subscriptions);

        _Subscriptions = subscriptions ?? new List<User>();
    }

    #endregion

    #region Instance Members

    public override NoInventoryAlert CreateBusinessRuleViolationDomainEvent(Item item)
    {
        return new NoInventoryAlert(item, _Subscriptions, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}