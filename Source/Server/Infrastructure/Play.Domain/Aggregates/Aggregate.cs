using Play.Core;
using Play.Domain.Entities;
using Play.Domain.Events;
using Play.Domain.Exceptions;
using Play.Randoms;

namespace Play.Domain.Aggregates;

public abstract class Aggregate<_TId> : Entity<_TId>, IAggregate, IEquatable<Aggregate<_TId>>, IEqualityComparer<Aggregate<_TId>> where _TId : IEquatable<_TId>
{
    #region Instance Members

    protected static string GenerateSimpleStringId() => Randomize.AlphaNumericSpecial.String(20);

    protected void Publish(DomainEvent domainEvent)
    {
        DomainEventBus.Publish(domainEvent);
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    protected void Enforce(IBusinessRule rule)
    {
        if (!rule.IsBroken())
            return;

        Publish(((IBusinessRule<Aggregate<_TId>>) rule).CreateBusinessRuleViolationDomainEvent(this));

        throw new BusinessRuleValidationException(rule);
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    protected void Enforce(IBusinessRule rule, Action brokenRuleCallbackAction)
    {
        if (!rule.IsBroken())
            return;

        brokenRuleCallbackAction.Invoke();
        Publish(((BusinessRule<Aggregate<_TId>, _TId>) rule).CreateBusinessRuleViolationDomainEvent(this));

        throw new BusinessRuleValidationException(rule);
    }

    protected Result<IBusinessRule> IsEnforced(IBusinessRule rule)
    {
        if (!rule.IsBroken())
            return new Result<IBusinessRule>(rule);

        Publish(((BusinessRule<Aggregate<_TId>, _TId>) rule).CreateBusinessRuleViolationDomainEvent(this));

        return new Result<IBusinessRule>(rule, rule.Message);
    }

    #endregion

    #region Equality

    public bool Equals(Aggregate<_TId>? other)
    {
        if (other is null)
            return false;

        // Cover for string reference equality
        if (other.Id is string otherId)
            return otherId == Id as string;

        // managed type has value equality for IEquatable
        if (other.Id!.Equals(Id))
            return true;

        return false;
    }

    public bool Equals(Aggregate<_TId>? x, Aggregate<_TId>? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(Aggregate<_TId> obj)
    {
        const int prime = 31;

        return obj.GetId().GetHashCode() * prime;
    }

    #endregion
}