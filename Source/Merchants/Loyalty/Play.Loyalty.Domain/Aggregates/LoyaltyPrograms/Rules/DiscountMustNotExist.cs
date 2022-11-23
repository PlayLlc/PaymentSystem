using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public class DiscountMustNotExist : BusinessRule<LoyaltyProgram, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Discount)} for the specified inventory item already exists";

    #endregion

    #region Constructor

    internal DiscountMustNotExist(IEnumerable<Discount> discounts, string itemId, string variationId)
    {
        _IsValid = discounts.All(a => !a.IsItemDiscounted(itemId, variationId));
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override PercentageWasInvalid CreateBusinessRuleViolationDomainEvent(LoyaltyProgram aggregate) => new PercentageWasInvalid(aggregate, this);

    #endregion
}