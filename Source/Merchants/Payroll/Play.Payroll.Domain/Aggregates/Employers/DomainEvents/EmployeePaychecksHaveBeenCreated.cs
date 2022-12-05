using Play.Domain.Events;

namespace Play.Payroll.Domain.Aggregates;

public record EmployeePaychecksHaveBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly Employer Employer;

    #endregion

    #region Constructor

    public EmployeePaychecksHaveBeenCreated(Employer employer) : base(
        $"The {nameof(Employer)} with the ID: [{employer.Id}] has created employee paychecks for this pay period;")
    {
        Employer = employer;
    }

    #endregion
}