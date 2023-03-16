using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.TimeClock.Domain.Enums;
using Play.TimeClock.Domain.ValueObject;

namespace Play.TimeClock.Domain.Aggregates;

public class EmployeeMustBeClockedOut : BusinessRule<Employee>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Employee)} must currently be {TimeClockStatuses.ClockedOut} in order to {TimeClockStatuses.ClockedIn};";

    #endregion

    #region Constructor

    internal EmployeeMustBeClockedOut(TimeClockStatus timeClockStatus)
    {
        _IsValid = timeClockStatus == TimeClockStatuses.ClockedOut;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployeeWasNotClockedOut CreateBusinessRuleViolationDomainEvent(Employee aggregate) => new(aggregate, this);

    #endregion
}