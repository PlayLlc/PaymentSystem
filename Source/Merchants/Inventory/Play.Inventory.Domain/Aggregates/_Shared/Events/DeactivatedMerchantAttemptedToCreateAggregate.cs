using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates.Events;

public record DeactivatedMerchantAttemptedToCreateAggregate<_Aggregate> : BrokenRuleOrPolicyDomainEvent<_Aggregate, SimpleStringId>
    where _Aggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    public readonly _Aggregate Aggregate;

    #endregion

    #region Constructor

    public DeactivatedMerchantAttemptedToCreateAggregate(_Aggregate aggregate, IBusinessRule rule) : base(aggregate, rule)
    {
        Aggregate = aggregate;
    }

    #endregion
}