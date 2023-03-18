using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Domain.Tests.TestDoubles.Aggregates.Shared.DomainEvents;

public record FirstCharacterWasNotCapitalized<_Aggregate> : BrokenRuleOrPolicyDomainEvent<_Aggregate, SimpleStringId>
    where _Aggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    public readonly _Aggregate Aggregate;

    #endregion

    #region Constructor

    public FirstCharacterWasNotCapitalized(_Aggregate aggregate, IBusinessRule rule) : base(aggregate, rule)
    {
        Aggregate = aggregate;
    }

    #endregion
}