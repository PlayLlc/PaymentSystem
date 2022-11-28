using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public class DiscountMustExist : BusinessRule<Programs, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Discount)} could not be updated because it does not exist";

    #endregion

    #region Constructor

    internal DiscountMustExist(DiscountProgram discountProgram, string discountId)
    {
        _IsValid = discountProgram.DoesDiscountExist(discountId);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override DiscountItemDoesNotExist CreateBusinessRuleViolationDomainEvent(Programs aggregate) => new(aggregate, this);

    #endregion
}