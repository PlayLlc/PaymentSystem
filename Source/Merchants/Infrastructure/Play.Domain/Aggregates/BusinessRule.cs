using Play.Domain.Events;

namespace Play.Domain.Aggregates;

public abstract class BusinessRule<_Aggregate, _TId> : IBusinessRule where _Aggregate : IAggregate where _TId : IEquatable<_TId>
{
    #region Instance Values

    public abstract string Message { get; }

    #endregion

    #region Instance Members

    // public abstract BusinessRuleViolationDomainEvent<_Aggregate, _TId> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate);

    public abstract DomainEvent CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate);

    public abstract bool IsBroken();

    #endregion
}