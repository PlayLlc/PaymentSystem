using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.TimeClock.Domain.Aggregates;

public record EmployeeAlreadyExists : BrokenRuleOrPolicyDomainEvent<Employee, SimpleStringId>
{
    #region Instance Values

    public readonly Employee Member;

    #endregion

    #region Constructor

    public EmployeeAlreadyExists(Employee member, IBusinessRule rule) : base(member, rule)
    {
        Member = member;
    }

    #endregion
}