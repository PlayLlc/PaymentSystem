﻿using Play.Domain.Events;
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

public record EmployeeTimeEntryHasBeenEdited : DomainEvent
{
    #region Instance Values

    public readonly Employee Employee;
    public readonly TimeEntry TimeEntry;
    public readonly string UserId;

    #endregion

    #region Constructor

    public EmployeeTimeEntryHasBeenEdited(Employee employee, TimeEntry timeEntry, string userId) : base(
        $"The {nameof(TimeEntry)} with the ID: [{timeEntry.Id}] has been updated for the {nameof(Employee)} with the ID: [{employee.Id}] by the {nameof(User)} with the ID: [{userId};")

    {
        Employee = employee;
        TimeEntry = timeEntry;
        UserId = userId;
    }

    #endregion
}