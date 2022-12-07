using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Payroll.Domain.Aggregates;

public record EmployerHasUndeliveredPaychecks : BrokenRuleOrPolicyDomainEvent<Employer, SimpleStringId>
{
    #region Instance Values

    public readonly Employer Aggregate;

    #endregion

    #region Constructor

    public EmployerHasUndeliveredPaychecks(Employer aggregate, IBusinessRule rule) : base(aggregate, rule)
    {
        Aggregate = aggregate;
    }

    #endregion
}