using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.ValueObjects;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public class CategoryMustHaveTheSameMerchant : BusinessRule<Item, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"The category must be associated with the same {nameof(Merchant)}";

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    internal CategoryMustHaveTheSameMerchant(Category category, string merchantId)
    {
        _IsValid = category.IsMerchantIdEqual(merchantId);
    }

    #endregion

    #region Instance Members

    public override CategoryMerchantDidNotMatch CreateBusinessRuleViolationDomainEvent(Item item)
    {
        return new CategoryMerchantDidNotMatch(item, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}