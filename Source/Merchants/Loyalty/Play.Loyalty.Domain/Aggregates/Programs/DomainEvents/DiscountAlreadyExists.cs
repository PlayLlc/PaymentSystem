using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record DiscountAlreadyExists : BrokenRuleOrPolicyDomainEvent<Programs, SimpleStringId>
{
    #region Instance Values

    public readonly Programs Programs;

    #endregion

    #region Constructor

    public DiscountAlreadyExists(Programs programs, IBusinessRule rule) : base(programs, rule)
    {
        Programs = programs;
    }

    #endregion
}