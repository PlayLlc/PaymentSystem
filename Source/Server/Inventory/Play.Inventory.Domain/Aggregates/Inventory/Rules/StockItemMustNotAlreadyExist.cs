using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public class StockItemMustNotAlreadyExist : BusinessRule<Inventory>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"The {nameof(StockItem)} could not be created because it already exists;";

    #endregion

    #region Constructor

    internal StockItemMustNotAlreadyExist(IEnumerable<StockItem> stockItems, string itemId)
    {
        _IsValid = stockItems.All(a => a.GetItemId() != itemId);
    }

    #endregion

    #region Instance Members

    public override AttemptedCreatingDuplicateStockItem CreateBusinessRuleViolationDomainEvent(Inventory inventory) => new(inventory, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}