using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.TimeClock.Domain.Enums;
using Play.TimeClock.Domain.ValueObject;

namespace Play.TimeClock.Domain.Aggregates;

public class EmployeeMustBeClockedIn : BusinessRule<Employee, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Employee)} must currently be {TimeClockStatuses.ClockedIn} in order to {TimeClockStatuses.ClockedOut};";

    #endregion

    #region Constructor

    internal EmployeeMustBeClockedIn(TimeClockStatus timeClockStatus)
    {
        _IsValid = timeClockStatus == TimeClockStatuses.ClockedOut;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployeeWasNotClockedIn CreateBusinessRuleViolationDomainEvent(Employee aggregate) => new(aggregate, this);

    #endregion
}