using Play.Domain.Events;
using Play.TimeClock.Domain.Entities;
using Play.TimeClock.Domain.Enums;

namespace Play.TimeClock.Domain.Aggregates;

public record EmployeeHasClockedOut : DomainEvent
{
    #region Instance Values

    public readonly Employee Employee;
    public readonly TimeEntry TimeEntry;

    #endregion

    #region Constructor

    public EmployeeHasClockedOut(Employee employee, TimeEntry timeEntry) : base(
        $"The {nameof(Employee)} with the ID: [{employee.Id}] has {nameof(TimeClockStatuses.ClockedIn)};")
    {
        Employee = employee;
        TimeEntry = timeEntry;
    }

    #endregion
}