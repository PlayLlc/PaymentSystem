using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Domain.Tests.Aggregates;

namespace Play.Domain.Tests.Events;

public record NameWasNotBrian : BrokenRuleOrPolicyDomainEvent<TestAggregate, SimpleStringId>
{
    #region Instance Values

    public readonly TestAggregate Item;

    #endregion

    #region Constructor

    public NameWasNotBrian(TestAggregate item, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
    }

    #endregion
}