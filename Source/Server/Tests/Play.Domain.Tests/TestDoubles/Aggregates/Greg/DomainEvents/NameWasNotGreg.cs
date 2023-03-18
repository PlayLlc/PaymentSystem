using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Domain.Tests.TestDoubles.Aggregates.Greg.DomainEvents;

public record NameWasNotGreg : BrokenRuleOrPolicyDomainEvent<Greg, SimpleStringId>
{
    #region Instance Values

    public readonly Greg Item;

    #endregion

    #region Constructor

    public NameWasNotGreg(Greg item, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
    }

    #endregion
}