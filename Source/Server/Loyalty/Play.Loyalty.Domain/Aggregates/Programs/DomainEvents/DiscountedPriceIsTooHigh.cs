using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountedPriceIsTooHigh : BrokenRuleOrPolicyDomainEvent<Programs, SimpleStringId>
{
    #region Instance Values

    public readonly Programs Programs;

    #endregion

    #region Constructor

    public DiscountedPriceIsTooHigh(Programs programs, IBusinessRule rule) : base(programs, rule)
    {
        Programs = programs;
    }

    #endregion
}