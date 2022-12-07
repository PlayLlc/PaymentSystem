using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Payroll.Domain.Aggregates;

public record EmployeeDoesNotExist : BrokenRuleOrPolicyDomainEvent<Employer, SimpleStringId>
{
    #region Instance Values

    public readonly Employer Aggregate;

    #endregion

    #region Constructor

    public EmployeeDoesNotExist(Employer aggregate, IBusinessRule rule) : base(aggregate, rule)
    {
        Aggregate = aggregate;
    }

    #endregion
}