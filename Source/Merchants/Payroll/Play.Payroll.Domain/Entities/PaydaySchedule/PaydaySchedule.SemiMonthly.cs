using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public partial class PaydaySchedule : Entity<SimpleStringId>
{
    #region Instance Members

    public static PaydaySchedule Create(string id, CreateSemiMonthlyPaySchedule command) =>
        new(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.Monthly), null, new DayOfTheMonth(command.FirstPayday),
            new DayOfTheMonth(command.SecondPayday));

    private PayPeriod GetNextSemiMonthlyPayPeriod() => throw new NotImplementedException();

    #endregion
}