using Play.Domain.Aggregates;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public class ItemVariationMustNotAlreadyExist : BusinessRule<Item, int>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"The variation being created must not already exist;";

    #endregion

    #region Constructor

    internal ItemVariationMustNotAlreadyExist(IEnumerable<Variation> variations, string name)
    {
        _IsValid = variations.All(a => a.GetName() != name);
    }

    #endregion

    #region Instance Members

    public override ItemVariationAlreadyExists CreateBusinessRuleViolationDomainEvent(Item item)
    {
        return new ItemVariationAlreadyExists(item, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}

public class StockItemMustNotAlreadyExist : BusinessRule<Inventory, string>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"The {nameof(StockItem)} could not be created because it already exists;";

    #endregion

    #region Constructor

    internal StockItemMustNotAlreadyExist(IEnumerable<StockItem> variations, string variationId)
    {
        _IsValid = variations.All(a => a.VariationId != variationId);
    }

    #endregion

    #region Instance Members

    public override StockItemAlreadyExists CreateBusinessRuleViolationDomainEvent(Inventory inventory)
    {
        return new StockItemAlreadyExists(inventory, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}