using Play.Domain.Aggregates;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public class ItemVariationMustExist : BusinessRule<Item, int>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"An attempt to update a variation failed because the variation does not exist";

    #endregion

    #region Constructor

    internal ItemVariationMustExist(IEnumerable<Variation> variations, string variationId)
    {
        _IsValid = variations.Any(a => a.GetId() == variationId);
    }

    #endregion

    #region Instance Members

    public override ItemVariationDoesNotExist CreateBusinessRuleViolationDomainEvent(Item item)
    {
        return new ItemVariationDoesNotExist(item, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}

public class StockItemMustExist : BusinessRule<Inventory, int>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"An attempt to update a {nameof(StockItem)} but failed because it does not exist";

    #endregion

    #region Constructor

    internal StockItemMustExist(IEnumerable<StockItem> stockItems, string variationId)
    {
        _IsValid = stockItems.Any(a => a.VariationId == variationId);
    }

    #endregion

    #region Instance Members

    public override StockItemDoesNotExist CreateBusinessRuleViolationDomainEvent(Inventory inventory)
    {
        return new StockItemDoesNotExist(inventory, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}