using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Domain.Aggregates;

public abstract class Aggregate<_TId> : Entity<_TId>, IEquatable<Aggregate<_TId>>, IEqualityComparer<Aggregate<_TId>>
{
    #region Constructor

    protected Aggregate()
    { }

    #endregion

    #region Instance Members

    protected void Raise(DomainEvent domainEvent)
    {
        // stuff?
        DomainEventBus.Publish(domainEvent);
    }

    public override string ToString()
    {
        return GetValueDetails();
    }

    private static string GetValueDetails()
    {
        throw new NotImplementedException();
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    protected void Enforce(IBusinessRule rule)
    {
        if (rule.IsBroken())
            throw new BusinessRuleValidationException(rule);
    }

    #endregion

    #region Equality

    public bool Equals(Aggregate<_TId>? other)
    {
        if (other is null)
            return false;

        if (other.GetId() == GetId())
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