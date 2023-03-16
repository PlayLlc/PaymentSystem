using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Aggregates;

public class EmployeeMustExist : BusinessRule<Employer>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Employer)} cannot update its {nameof(Employee)} collection because the {nameof(Employee)} does not exist;";

    #endregion

    #region Constructor

    internal EmployeeMustExist(string employeeId, IEnumerable<Employee> employees)
    {
        _IsValid = employees.Any(a => a.Id == employeeId);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployeeDoesNotExist CreateBusinessRuleViolationDomainEvent(Employer aggregate) => new(aggregate, this);

    #endregion
}