using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates._Shared.DomainEvents;

public record DeactivatedUserAttemptedToUpdateAggregate<_Aggregate> : BrokenRuleOrPolicyDomainEvent<_Aggregate, SimpleStringId>
    where _Aggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    public readonly _Aggregate Aggregate;

    #endregion

    #region Constructor

    public DeactivatedUserAttemptedToUpdateAggregate(_Aggregate aggregate, IBusinessRule rule) : base(aggregate, rule)
    {
        Aggregate = aggregate;
    }

    #endregion
}