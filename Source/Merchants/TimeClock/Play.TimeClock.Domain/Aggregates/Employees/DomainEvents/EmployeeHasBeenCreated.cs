using Play.Domain.Events;
using Play.TimeClock.Domain.Entities;

namespace Play.TimeClock.Domain.Aggregates;

public record EmployeeHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly Employee Employee;

    #endregion

    #region Constructor

    public EmployeeHasBeenCreated(Employee employee, string merchantId, string userId) : base(
        $"The {nameof(Employee)} with the ID: [{employee.Id}] has been created for the {nameof(Merchant)} witht he ID: [{merchantId}] by the {nameof(User)} with the ID: [{userId}];")

    {
        Employee = employee;
    }

    #endregion
}