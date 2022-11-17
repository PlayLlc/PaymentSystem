using Play.Domain.Aggregates;
using Play.Inventory.Contracts.Enums;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain;

public class StockActionMustAddQuantity : BusinessRule<Item, int>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"An incorrect {nameof(StockAction)} was attempted;";

    #endregion

    #region Constructor

    internal StockActionMustAddQuantity(StockActions stockAction)
    {
        _IsValid = (stockAction == StockActions.Restock) || (stockAction == StockActions.Return);
    }

    #endregion

    #region Instance Members

    public override StockActionWasIncorrect CreateBusinessRuleViolationDomainEvent(Item item)
    {
        return new StockActionWasIncorrect(item, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}