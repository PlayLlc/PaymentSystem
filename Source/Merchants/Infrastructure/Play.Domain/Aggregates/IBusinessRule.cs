using Play.Domain.Events;

namespace Play.Domain.Aggregates;

public interface IBusinessRule
{
    #region Instance Values

    string Message { get; }

    #endregion
}

public abstract class BusinessRule<_Aggregate, _TId> : IBusinessRule where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Instance Values

    public abstract string Message { get; }

    #endregion

    #region Instance Members

    public abstract BusinessRuleViolationDomainEvent<_Aggregate, _TId> CreateBusinessRuleViolationDomainEvent();
    public abstract bool IsBroken();

    #endregion
}