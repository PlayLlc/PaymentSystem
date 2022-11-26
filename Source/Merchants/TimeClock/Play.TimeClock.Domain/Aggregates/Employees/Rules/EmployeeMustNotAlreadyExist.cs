using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.TimeClock.Domain.Services;

namespace Play.TimeClock.Domain.Aggregates;

public class EmployeeMustNotAlreadyExist : BusinessRule<Employee, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Employee)} is responsible for their {nameof(TimeClock)} management and must clock themselves in and out;";

    #endregion

    #region Constructor

    internal EmployeeMustNotAlreadyExist(IEnsureEmployeeDoesNotExist uniqueEmployeeChecker, string merchantId, string userId)
    {
        _IsValid = !uniqueEmployeeChecker.DoesEmployeeAlreadyExist(merchantId, userId);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployeeAlreadyExists CreateBusinessRuleViolationDomainEvent(Employee aggregate) => new(aggregate, this);

    #endregion
}