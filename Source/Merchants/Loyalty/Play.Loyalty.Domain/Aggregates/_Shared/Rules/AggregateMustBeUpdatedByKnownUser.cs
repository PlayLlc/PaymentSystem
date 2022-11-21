using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Aggregates._Shared.DomainEvents;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates._Shared.Rules;

public class AggregateMustBeUpdatedByKnownUser<_Aggregate> : BusinessRule<_Aggregate, SimpleStringId> where _Aggregate : Aggregate<SimpleStringId>
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

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    public override AggregateUpdateWasAttemptedByUnknownUser<_Aggregate> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate)
    {
        return new AggregateUpdateWasAttemptedByUnknownUser<_Aggregate>(aggregate, this);
    }

    #endregion
}