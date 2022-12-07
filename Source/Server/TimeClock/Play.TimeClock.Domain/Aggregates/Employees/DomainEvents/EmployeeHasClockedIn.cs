using Play.Domain.Events;
using Play.TimeClock.Domain.Enums;

namespace Play.TimeClock.Domain.Aggregates;

public record EmployeeHasClockedIn : DomainEvent
{
    #region Instance Values

    public readonly Employee Employee;

    #endregion

    #region Constructor

    public EmployeeHasClockedIn(Employee employee) : base($"The {nameof(Employee)} with the ID: [{employee.Id}] has {nameof(TimeClockStatuses.ClockedIn)};")
    {
        Employee = employee;
    }

    #endregion
}