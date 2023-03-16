using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public class AggregateMustBeUpdatedByKnownUser<_Aggregate> : BusinessRule<_Aggregate> where _Aggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message =>
        $"The {typeof(_Aggregate).Name} can only be updated by a {nameof(User)} that belongs to the same {nameof(Merchant)} organization;";

    #endregion

    #region Constructor

    internal AggregateMustBeUpdatedByKnownUser(string merchantId, User user)
    {
        _IsValid = user.DoesUserBelongToMerchant(merchantId);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override AggregateUpdateWasAttemptedByUnknownUser<_Aggregate> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate) => new(aggregate, this);

    #endregion
}