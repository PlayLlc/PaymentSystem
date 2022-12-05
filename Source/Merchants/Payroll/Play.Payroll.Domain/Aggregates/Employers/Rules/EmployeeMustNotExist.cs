using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Aggregates;

public class EmployeeMustNotExist : BusinessRule<Employer, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message =>
        $"The {nameof(Employer)} cannot create the {nameof(Employee)} because an {nameof(Employee)} with the same UserId already exists;";

    #endregion

    #region Constructor

    internal EmployeeMustNotExist(string userId, IEnumerable<Employee> employees)
    {
        _IsValid = employees.Any(a => a.GetUserId() == userId);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployeeDoesNotExist CreateBusinessRuleViolationDomainEvent(Employer aggregate) => new(aggregate, this);

    #endregion
}