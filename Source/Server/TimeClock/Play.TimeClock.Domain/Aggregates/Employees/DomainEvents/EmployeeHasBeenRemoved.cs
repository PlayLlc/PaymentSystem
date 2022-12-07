using Play.Domain.Events;

namespace Play.TimeClock.Domain.Aggregates;

public record EmployeeHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly Employee Employee;

    #endregion

    #region Constructor

    public EmployeeHasBeenRemoved(Employee employee) : base($"The {nameof(Employee)} with the ID: [{employee.Id}] has been removed;")

    {
        Employee = employee;
    }

    #endregion
}