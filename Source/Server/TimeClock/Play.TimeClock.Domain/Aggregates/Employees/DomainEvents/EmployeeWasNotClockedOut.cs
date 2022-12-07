using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.TimeClock.Domain.Aggregates;

public record EmployeeWasNotClockedOut : BrokenRuleOrPolicyDomainEvent<Employee, SimpleStringId>
{
    #region Instance Values

    public readonly Employee Member;

    #endregion

    #region Constructor

    public EmployeeWasNotClockedOut(Employee member, IBusinessRule rule) : base(member, rule)
    {
        Member = member;
    }

    #endregion
}