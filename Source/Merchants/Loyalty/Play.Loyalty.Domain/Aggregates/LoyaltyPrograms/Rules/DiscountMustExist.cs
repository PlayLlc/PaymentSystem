using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public class DiscountMustExist : BusinessRule<LoyaltyProgram, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Discount)} could not be updated because it does not exist";

    #endregion

    #region Constructor

    internal DiscountMustExist(IEnumerable<Discount> discounts, string discountId)
    {
        _IsValid = discounts.Any(a => a.Id == discountId);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    public override DiscountItemDoesNotExist CreateBusinessRuleViolationDomainEvent(LoyaltyProgram aggregate)
    {
        return new DiscountItemDoesNotExist(aggregate, this);
    }

    #endregion
}