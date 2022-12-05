using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Globalization.Currency;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Domain.Aggregates;

public partial class Employer : Aggregate<SimpleStringId>
{
    #region Instance Members

    public async Task CutPaychecks(IISendAchTransfers achClient, CutChecks commands)
    {
        // Enforce
        PayPeriod payPeriod = new(commands.PayPeriod);

        // HACK: TRANSACTIONAL CONSISTENCY!!!!!!
        // BUG: TRANSACTIONAL CONSISTENCY!!!!!
        // HACK: TRANSACTIONAL CONSISTENCY!!!!!!
        foreach (var employee in _Employees)
        {
            employee.AddPaycheck(CutPaycheck(payPeriod, employee));

            // TODO: Move this to NServiceBus layer for transactional consistency
            await employee.TryDispursingUndeliveredChecks(achClient).ConfigureAwait(false);
        }

        // Publish
    }

    private Paycheck CutPaycheck(PayPeriod payPeriod, Employee employee)
    {
        TimeSheet timeSheet = TimeSheet.Create(GenerateSimpleStringId(), employee.Id, payPeriod, employee.GetTimeEntries(payPeriod));
        Money earnedWage = employee.CalculatePaycheckEarnings(timeSheet);

        return Paycheck.Create(GenerateSimpleStringId(), employee.Id, earnedWage, timeSheet, payPeriod);
    }

    #endregion
}