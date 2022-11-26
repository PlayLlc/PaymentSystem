using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.TimeClock.Domain.Aggregates.Employees.DomainEvents;
using Play.TimeClock.Domain.Entities;

namespace Play.TimeClock.Domain.Aggregates.Employees.Rules;

public class EmployeeMustClockThemselvesInAndOut : BusinessRule<Employee, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Employee)} is responsible for their {nameof(TimeClock)} management and must clock themselves in and out;";

    #endregion

    #region Constructor

    internal EmployeeMustClockThemselvesInAndOut(User user, string userId)
    {
        _IsValid = user.GetId() == userId;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override UnauthorizedUserAttemptedToUpdateEmployeeTimeClock CreateBusinessRuleViolationDomainEvent(Employee aggregate) => new(aggregate, this);

    #endregion
}