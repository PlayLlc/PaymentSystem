using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Entitiesd;

namespace Play.Loyalty.Domain.Aggregates;

public class DiscountMustNotExist : BusinessRule<LoyaltyProgram, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Discount)} for the specified inventory item already exists";

    #endregion

    #region Constructor

    internal DiscountMustNotExist(DiscountsProgram discountsProgram, string itemId, string variationId)
    {
        _IsValid = !discountsProgram.DoesDiscountExist(itemId, variationId);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override DiscountAlreadyExists CreateBusinessRuleViolationDomainEvent(LoyaltyProgram aggregate) => new(aggregate, this);

    #endregion
}