using Play.Domain.Entities;
using Play.Domain.Events;
using Play.Randoms;

namespace Play.Domain.Aggregates;

public interface IAggregate
{ }

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
        // stuff?
        DomainEventBus.Publish(domainEvent);
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    protected void Enforce(IBusinessRule<Aggregate<_TId>, _TId> rule)
    {
        if (rule.IsBroken())
        {
            Publish(rule.CreateBusinessRuleViolationDomainEvent());

            throw new BusinessRuleValidationException(rule);
        }
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