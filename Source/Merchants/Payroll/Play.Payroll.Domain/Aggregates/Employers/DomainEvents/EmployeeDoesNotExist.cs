using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Payroll.Domain.Aggregates.Employers;

namespace Play.Loyalty.Domain.Aggregatesdd;

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

public record EmployeeHasUndeliveredPaychecks : BrokenRuleOrPolicyDomainEvent<Employer, SimpleStringId>
{
    #region Instance Values

    public readonly Employer Aggregate;

    #endregion

    #region Constructor

    public EmployeeHasUndeliveredPaychecks(Employer aggregate, IBusinessRule rule) : base(aggregate, rule)
    {
        Aggregate = aggregate;
    }

    #endregion
}