using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public class ItemVariationMustExist : BusinessRule<Item, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => "An attempt to update a variation failed because the variation does not exist";

    #endregion

    #region Constructor

    internal ItemVariationMustExist(IEnumerable<Variation> variations, string variationId)
    {
        _IsValid = variations.Any(a => a.GetId() == variationId);
    }

    #endregion

    #region Instance Members

    public override ItemVariationDoesNotExist CreateBusinessRuleViolationDomainEvent(Item item) => new(item, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}