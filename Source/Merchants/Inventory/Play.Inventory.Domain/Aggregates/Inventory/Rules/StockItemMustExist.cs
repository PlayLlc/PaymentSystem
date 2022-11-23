using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public class StockItemMustExist : BusinessRule<Inventory, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"An attempt to update a {nameof(StockItem)} but failed because it does not exist";

    #endregion

    #region Constructor

    internal StockItemMustExist(IEnumerable<StockItem> stockItems, string variationId)
    {
        _IsValid = stockItems.Any(a => a.GetVariationId() == variationId);
    }

    #endregion

    #region Instance Members

    public override StockItemDoesNotExist CreateBusinessRuleViolationDomainEvent(Inventory inventory) => new StockItemDoesNotExist(inventory, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}