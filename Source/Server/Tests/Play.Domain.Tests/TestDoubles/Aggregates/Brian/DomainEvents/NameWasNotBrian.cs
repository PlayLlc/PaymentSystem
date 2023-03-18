using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Domain.Tests.TestDoubles.Aggregates.Brian.DomainEvents;

public record NameWasNotBrian : BrokenRuleOrPolicyDomainEvent<Brian, SimpleStringId>
{
    #region Instance Values

    public readonly Brian Item;

    #endregion

    #region Constructor

    public NameWasNotBrian(Brian item, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
    }

    #endregion
}