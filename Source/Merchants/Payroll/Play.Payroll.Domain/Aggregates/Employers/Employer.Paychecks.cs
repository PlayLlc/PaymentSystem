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

    /// <summary>
    ///     A scheduler will routinely look to see if today is a payday for this employer
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ValueObjectException"></exception>
    public bool IsTodayPayday() => _PaydaySchedule.IsTodayPayday(GetLastRecordedPayPeriod());

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="Play.Domain.Exceptions.BusinessRuleValidationException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public void CutPaychecks(CutChecks command)
    {
        PayPeriod payPeriod = GetPayPeriod(new DateTimeUtc(command.Payday).AsShortDate());
        Enforce(new PayPeriodMustHaveEnded(payPeriod));

        foreach (Employee employee in _Employees)
            employee.AddPaycheck(CutPaycheck(payPeriod, employee));

        Publish(new EmployeePaychecksHaveBeenCreated(this));
    }

    public async Task DistributeUndeliveredChecks(IISendAchTransfers achClient)
    {
        foreach (Employee employee in _Employees)
            await employee.DisperseUndeliveredChecks(achClient).ConfigureAwait(false);

        Publish(new EmployeePaychecksHaveBeenDelivered(this));
    }

    /// <exception cref="ValueObjectException"></exception>
    internal PayPeriod GetPayPeriod(ShortDate payday) => new(GenerateSimpleStringId(), _PaydaySchedule.GetPayPeriodDateRange(payday));

    private Paycheck CutPaycheck(PayPeriod payPeriod, Employee employee)
    {
        TimeSheet timeSheet = TimeSheet.Create(GenerateSimpleStringId(), employee.Id, payPeriod, employee.GetTimeEntries(payPeriod));
        Money earnedWage = employee.CalculatePaycheckEarnings(timeSheet);

        return Paycheck.Create(GenerateSimpleStringId(), employee.Id, earnedWage, timeSheet, payPeriod);
    }

    private DateRange? GetLastRecordedPayPeriod() => _Employees?.Max(a => a?.GetLatestPaycheck())?.GetPayPeriod()?.GetDateRange();

    #endregion
}