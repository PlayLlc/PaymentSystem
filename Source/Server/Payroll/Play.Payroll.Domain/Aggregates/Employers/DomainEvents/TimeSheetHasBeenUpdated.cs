using Play.Domain.Events;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Aggregates;

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