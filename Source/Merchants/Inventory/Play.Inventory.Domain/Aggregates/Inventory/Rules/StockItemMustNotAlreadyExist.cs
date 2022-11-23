using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public class StockItemMustNotAlreadyExist : BusinessRule<Inventory, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"The {nameof(StockItem)} could not be created because it already exists;";

    #endregion

    #region Constructor

    internal StockItemMustNotAlreadyExist(IEnumerable<StockItem> variations, string variationId)
    {
        _IsValid = variations.All(a => a.GetVariationId() != variationId);
    }

    #endregion

    #region Instance Members

    public override StockItemAlreadyExists CreateBusinessRuleViolationDomainEvent(Inventory inventory) => new StockItemAlreadyExists(inventory, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}