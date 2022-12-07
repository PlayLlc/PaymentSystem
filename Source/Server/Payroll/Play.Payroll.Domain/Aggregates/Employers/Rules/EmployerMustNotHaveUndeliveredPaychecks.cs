using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;

namespace Play.Payroll.Domain.Aggregates;

public class EmployerMustNotHaveUndeliveredPaychecks : BusinessRule<Employer, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Employer)} cannot be removed because there are one or more employees with undelivered paychecks;";

    #endregion

    #region Constructor

    internal EmployerMustNotHaveUndeliveredPaychecks(Employer employer)
    {
        _IsValid = !employer.AnyUndeliveredPaychecks();
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployerHasUndeliveredPaychecks CreateBusinessRuleViolationDomainEvent(Employer aggregate) => new(aggregate, this);

    #endregion
}