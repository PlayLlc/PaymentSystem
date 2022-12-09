using Play.Domain.Events;
using Play.Payroll.Domain.Aggregates._External;

namespace Play.Payroll.Domain.Aggregates;

public record EmployeeHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly Employer Employer;
    public readonly string EmployeeId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public EmployeeHasBeenCreated(Employer employer, string employeeId, string userId) : base(
        $"The {nameof(Employer)} with the ID: [{employer.Id}] has created the {nameof(Employee)} with the ID: [{employeeId}];")
    {
        Employer = employer;
        EmployeeId = employeeId;
        UserId = userId;
    }

    #endregion
}