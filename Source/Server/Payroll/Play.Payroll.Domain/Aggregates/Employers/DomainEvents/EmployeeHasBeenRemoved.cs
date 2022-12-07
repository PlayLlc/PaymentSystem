using Play.Domain.Events;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Aggregates;

public record EmployeeHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly Employer Employer;
    public readonly string EmployeeId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public EmployeeHasBeenRemoved(Employer employer, string employeeId, string userId) : base(
        $"The {nameof(Employer)} with the ID: [{employer.Id}] has been removed the {nameof(Employee)} with the ID: [{employeeId}];")
    {
        Employer = employer;
        EmployeeId = employeeId;
        UserId = userId;
    }

    #endregion
}

public record TimeSheetHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Employer Employer;

    #endregion

    #region Constructor

    public TimeSheetHasBeenUpdated(Employer employer, string timeSheetId) : base($"The {nameof(TimeSheet)} with the ID: [{timeSheetId}] has been updated;")
    {
        Employer = employer;
    }

    #endregion
}

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