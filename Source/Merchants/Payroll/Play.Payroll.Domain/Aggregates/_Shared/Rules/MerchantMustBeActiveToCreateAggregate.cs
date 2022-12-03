using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

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

    public override bool IsBroken() => !_IsValid;

    public override DeactivatedMerchantAttemptedToCreateAggregate<_Aggregate> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate) =>
        new(aggregate, this);

    #endregion
}