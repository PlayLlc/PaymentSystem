using Play.Domain.Events;

namespace Play.Domain.Aggregates;

public interface IBusinessRule<in _Aggregate, _TId> : IBusinessRule where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Instance Members

    public DomainEvent CreateBusinessRuleViolationDomainEvent(_Aggregate merchant);

    #endregion
}

public abstract class BusinessRule<_Aggregate, _TId> : IBusinessRule<_Aggregate, _TId> where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Instance Values

    public abstract string Message { get; }

    #endregion

    #region Instance Members

    public abstract DomainEvent CreateBusinessRuleViolationDomainEvent(_Aggregate merchant);

    public abstract bool IsBroken();

    #endregion
}