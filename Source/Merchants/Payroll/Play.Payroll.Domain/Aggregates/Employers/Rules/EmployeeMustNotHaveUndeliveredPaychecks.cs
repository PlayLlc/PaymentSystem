using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Aggregates;

public class EmployeeMustNotHaveUndeliveredPaychecks : BusinessRule<Employer, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message =>
        $"The {nameof(Employer)} cannot remove the {nameof(Employee)} because the {nameof(Employee)} still has paychecks that have yet to be delivered;";

    #endregion

    #region Constructor

    internal EmployeeMustNotHaveUndeliveredPaychecks(Employee employee)
    {
        _IsValid = !employee.GetUndeliveredPaychecks().Any();
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployeeHasUndeliveredPaychecks CreateBusinessRuleViolationDomainEvent(Employer aggregate) => new(aggregate, this);

    #endregion
}