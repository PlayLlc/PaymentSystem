﻿using Play.Core;
using Play.Domain.Entities;
using Play.Domain.Events;
using Play.Randoms;

namespace Play.Domain.Aggregates;

public abstract class Aggregate<_TId> : Entity<_TId>, IAggregate, IEquatable<Aggregate<_TId>>, IEqualityComparer<Aggregate<_TId>> where _TId : IEquatable<_TId>
{
    #region Constructor

    protected Aggregate()
    { }

    #endregion

    #region Instance Members

    protected static string GenerateSimpleStringId()
    {
        return Randomize.AlphaNumericSpecial.String(20);
    }

    protected void Publish(DomainEvent domainEvent)
    {
        DomainEventBus.Publish(domainEvent);
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    protected void Enforce(IBusinessRule rule)
    {
        if (rule.IsBroken())
            Publish(((BusinessRule<Aggregate<_TId>, _TId>) rule).CreateBusinessRuleViolationDomainEvent(this));

        throw new BusinessRuleValidationException(rule);
    }

    protected Result GetEnforcementResult(IBusinessRule rule)
    {
        if (!rule.IsBroken())
            return new Result();

        Publish(((BusinessRule<Aggregate<_TId>, _TId>) rule).CreateBusinessRuleViolationDomainEvent(this));

        return new Result(rule.Message);
    }

    #endregion

    #region Equality

    public bool Equals(Aggregate<_TId>? other)
    {
        if (other is null)
            return false;

        if (other.GetId()!.Equals(GetId()))
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