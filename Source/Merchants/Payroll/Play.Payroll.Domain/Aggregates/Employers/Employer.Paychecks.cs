using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.Services;
using Play.Globalization.Time;

namespace Play.Payroll.Domain.Aggregates;

public partial class Employer : Aggregate<SimpleStringId>
{
    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public bool IsTodayPayday() => _PaydaySchedule.IsTodayPayday(GetLastRecordedPayPeriod());

    // HACK: This method is relative and probably will add ambiguity and errors in the implementation DELETE if possible
    /// <exception cref="ValueObjectException"></exception>
    public PayPeriod GetNextPayPeriod() => new(GenerateSimpleStringId(), _PaydaySchedule.GetNextPayPeriod(GetLastRecordedPayPeriod()));

    /// <exception cref="ValueObjectException"></exception>
    public PayPeriod GetPayPeriod(ShortDate payday) => new(GenerateSimpleStringId(), _PaydaySchedule.GetPayPeriodDateRange(payday));

    public async Task TryDispursingUndeliveredChecks(IISendAchTransfers achClient)
    {
        foreach (var employee in _Employees)
            await employee.TryDispursingUndeliveredChecks(achClient).ConfigureAwait(false);

        Publish(new EmployeePaychecksHaveBeenDelivered(this));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="Play.Domain.Exceptions.BusinessRuleValidationException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public void CutPaychecks(CutChecks commands)
    {
        PayPeriod payPeriod = GetPayPeriod(new DateTimeUtc(commands.Payday).AsShortDate());
        Enforce(new PayPeriodMustHaveEnded(payPeriod));

        // ENFORCE OTHER SHIT

        foreach (var employee in _Employees)
            employee.AddPaycheck(CutPaycheck(payPeriod, employee));

        Publish(new EmployeePaychecksHaveBeenCreated(this));
    }

    private Paycheck CutPaycheck(PayPeriod payPeriod, Employee employee)
    {
        TimeSheet timeSheet = TimeSheet.Create(GenerateSimpleStringId(), employee.Id, payPeriod, employee.GetTimeEntries(payPeriod));
        Money earnedWage = employee.CalculatePaycheckEarnings(timeSheet);

        return Paycheck.Create(GenerateSimpleStringId(), employee.Id, earnedWage, timeSheet, payPeriod);
    }

    private DateRange? GetLastRecordedPayPeriod() => _Employees?.Max(a => a?.GetLatestPaycheck())?.GetPayPeriod()?.GetDateRange();

    #endregion
}