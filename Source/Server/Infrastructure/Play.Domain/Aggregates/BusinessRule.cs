using Play.Domain.Events;

namespace Play.Domain.Aggregates;

public interface IBusinessRule
{
    #region Instance Values

    string Message { get; }

    #endregion

    #region Instance Members

    public bool IsBroken();

    #endregion
}

public interface IBusinessRule<_Aggregate> : IBusinessRule where _Aggregate : IAggregate
{
    #region Instance Members

    public DomainEvent CreateBusinessRuleViolationDomainEvent(_Aggregate merchant);

    #endregion
}

public abstract class BusinessRule<_Aggregate> : IBusinessRule<_Aggregate>
    where _Aggregate : IAggregate // : IBusinessRule<_Aggregate> where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Instance Values

    public abstract string Message { get; }

    #endregion

    #region Instance Members

    // public abstract BusinessRuleViolationDomainEvent<_Aggregate, _TId> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate);

    public abstract DomainEvent CreateBusinessRuleViolationDomainEvent(_Aggregate merchant);

    public abstract bool IsBroken();

    #endregion
}