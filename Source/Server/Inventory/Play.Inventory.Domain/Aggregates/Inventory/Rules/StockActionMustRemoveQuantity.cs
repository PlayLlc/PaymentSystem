using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Contracts.Enums;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

public class StockActionMustRemoveQuantity : BusinessRule<Inventory, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"An incorrect {nameof(StockAction)} was attempted;";

    #endregion

    #region Constructor

    internal StockActionMustRemoveQuantity(StockActions stockAction)
    {
        _IsValid = (stockAction == StockActions.Shrinkage) || (stockAction == StockActions.Sold);
    }

    #endregion

    #region Instance Members

    public override StockActionWasIncorrect CreateBusinessRuleViolationDomainEvent(Inventory item) => new(item, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}