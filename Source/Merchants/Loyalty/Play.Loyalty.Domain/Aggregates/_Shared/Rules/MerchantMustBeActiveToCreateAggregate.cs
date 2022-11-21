using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Aggregates._Shared.DomainEvents;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates._Shared.Rules;

public class MerchantMustBeActiveToCreateAggregate<_Aggregate> : BusinessRule<_Aggregate, SimpleStringId> where _Aggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {typeof(_Aggregate)} cannot be created because the {nameof(Merchant)} has been deactivated;";

    #endregion

    #region Constructor

    internal MerchantMustBeActiveToCreateAggregate(Merchant merchant)
    {
        _IsValid = merchant.IsActive;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    public override DeactivatedMerchantAttemptedToCreateAggregate<_Aggregate> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate)
    {
        return new DeactivatedMerchantAttemptedToCreateAggregate<_Aggregate>(aggregate, this);
    }

    #endregion
}