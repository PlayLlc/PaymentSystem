using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Aggregates._Shared.DomainEvents;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates._Shared.Rules;

public class UserMustBeActiveToUpdateAggregate<_Aggregate> : BusinessRule<_Aggregate, SimpleStringId> where _Aggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(User)} cannot update the {typeof(_Aggregate).Name} because the {nameof(User)} has been deactivated;";

    #endregion

    #region Constructor

    internal UserMustBeActiveToUpdateAggregate(User user)
    {
        _IsValid = user.IsActive;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    public override DeactivatedUserAttemptedToUpdateAggregate<_Aggregate> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate)
    {
        return new DeactivatedUserAttemptedToUpdateAggregate<_Aggregate>(aggregate, this);
    }

    #endregion
}