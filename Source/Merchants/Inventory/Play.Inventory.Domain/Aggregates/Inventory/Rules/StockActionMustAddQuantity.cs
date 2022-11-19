using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Contracts.Enums;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

public class StockActionMustAddQuantity : BusinessRule<Inventory, SimpleStringId>
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

    public override StockActionWasIncorrect CreateBusinessRuleViolationDomainEvent(Inventory inventory)
    {
        return new StockActionWasIncorrect(inventory, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}