using Play.Domain.Events;
using Play.Payroll.Domain.Aggregates._External;

namespace Play.Payroll.Domain.Aggregates;

public record EmployeeCompensationHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Employer Employer;

    #endregion

    #region Constructor

    public EmployeeCompensationHasBeenUpdated(Employer employer, string employeeId) : base(
        $"The compensation for {nameof(Employee)} with the ID: [{employeeId}] has been updated;")
    {
        Employer = employer;
    }

    #endregion
}