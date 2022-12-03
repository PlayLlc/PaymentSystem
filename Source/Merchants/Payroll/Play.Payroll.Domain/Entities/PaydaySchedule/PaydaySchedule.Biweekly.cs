using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public partial class PaydaySchedule : Entity<SimpleStringId>
{
    #region Instance Members

    public static PaydaySchedule Create(string id, CreateBiweeklyPaySchedule command) =>
        new(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.Biweekly), new DayOfTheWeek(command.Payday), null, null);

    private PayPeriod GetNextBiweeklyPayPeriod() => throw new NotImplementedException();

    #endregion
}