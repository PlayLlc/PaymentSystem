using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public class ItemVariationMustNotAlreadyExist : BusinessRule<Item, SimpleStringId>
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