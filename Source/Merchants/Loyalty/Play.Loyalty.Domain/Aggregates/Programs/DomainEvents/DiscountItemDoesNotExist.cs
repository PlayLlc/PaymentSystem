using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountItemDoesNotExist : BrokenRuleOrPolicyDomainEvent<Programs, SimpleStringId>
{
    #region Instance Values

    public readonly Programs Programs;

    #endregion

    #region Constructor

    public DiscountItemDoesNotExist(Programs programs, IBusinessRule rule) : base(programs, rule)
    {
        Programs = programs;
    }

    #endregion
}