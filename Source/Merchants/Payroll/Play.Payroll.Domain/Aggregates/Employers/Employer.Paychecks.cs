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
    public bool IsTodayPayday() => _PaydaySchedule.IsTodayPayday(GetLatestPayPeriod());

    /// <exception cref="ValueObjectException"></exception>
    public PayPeriod GetNextPayPeriod() => new(GenerateSimpleStringId(), _PaydaySchedule.GetNextPayPeriod(GetLatestPayPeriod()));

    public async Task TryDispursingUndeliveredChecks(IISendAchTransfers achClient)
    {
        foreach (var employee in _Employees)
            await employee.TryDispursingUndeliveredChecks(achClient).ConfigureAwait(false);

        Publish(new EmployeePaychecksHaveBeenDelivered(this));
    }

    public async Task CutPaychecks(IISendAchTransfers achClient, CutChecks commands)
    {
        // Enforce
        PayPeriod payPeriod = new(commands.PayPeriod);

        // HACK: TRANSACTIONAL CONSISTENCY!!!!!!
        // BUG: TRANSACTIONAL CONSISTENCY!!!!!
        // HACK: TRANSACTIONAL CONSISTENCY!!!!!!
        foreach (var employee in _Employees)
            employee.AddPaycheck(CutPaycheck(payPeriod, employee));

        // TODO: Move this to NServiceBus layer for transactional consistency
        //await employee.TryDispursingUndeliveredChecks(achClient).ConfigureAwait(false);
        // Publish
    }

    private Paycheck CutPaycheck(PayPeriod payPeriod, Employee employee)
    {
        TimeSheet timeSheet = TimeSheet.Create(GenerateSimpleStringId(), employee.Id, payPeriod, employee.GetTimeEntries(payPeriod));
        Money earnedWage = employee.CalculatePaycheckEarnings(timeSheet);

        return Paycheck.Create(GenerateSimpleStringId(), employee.Id, earnedWage, timeSheet, payPeriod);
    }

    private DateRange? GetLatestPayPeriod() => _Employees?.Max(a => a?.GetLatestPaycheck())?.GetPayPeriod()?.GetDateRange();

    #endregion
}