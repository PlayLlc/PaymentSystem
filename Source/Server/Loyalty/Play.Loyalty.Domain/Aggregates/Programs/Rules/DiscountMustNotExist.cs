using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public class DiscountMustNotExist : BusinessRule<Programs>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Discount)} for the specified inventory item already exists";

    #endregion

    #region Constructor

    internal DiscountMustNotExist(DiscountProgram discountProgram, string itemId, string variationId)
    {
        _IsValid = !discountProgram.DoesDiscountExist(itemId, variationId);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override DiscountAlreadyExists CreateBusinessRuleViolationDomainEvent(Programs aggregate) => new(aggregate, this);

    #endregion
}