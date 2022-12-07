using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Time;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Aggregates;

public class PayPeriodMustHaveEnded : BusinessRule<Employer, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Employer)} cannot create paychecks because the {nameof(PayPeriod)} has not yet ended;";

    #endregion

    #region Constructor

    internal PayPeriodMustHaveEnded(PayPeriod payPeriod)
    {
        _IsValid = DateTimeUtc.Now.AsShortDate() <= payPeriod.GetDateRange().GetEndDate().AsShortDate();
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override PayPeriodHasNotEnded CreateBusinessRuleViolationDomainEvent(Employer aggregate) => new(aggregate, this);

    #endregion
}