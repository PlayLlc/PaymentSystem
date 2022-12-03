using Play.Core;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public partial class PaydaySchedule : Entity<SimpleStringId>
{
    #region Instance Members

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    public static PaydaySchedule Create(SimpleStringId id, CreateWeeklyPaySchedule command) =>
        new(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.Weekly), new DayOfTheWeek(command.DayOfTheWeek), null, null);

    // WARNING: I haven't tested this logic and it's likely wrong
    private PayPeriod GetNextWeeklyPayPeriod()
    {
        DateTimeUtc? lastPayday;
        DateTimeUtc today = DateTimeUtc.Now;
        DayOfTheWeek dayOfTheWeek = new(today.GetDayOfTheWeek());

        if (dayOfTheWeek == _WeeklyPayday!)
            return new PayPeriod("", today, today.AddDays(7));

        if (dayOfTheWeek > _WeeklyPayday!)
        {
            lastPayday = today.AddDays(dayOfTheWeek - _WeeklyPayday!);

            return new PayPeriod("", lastPayday!.Value, lastPayday.Value.AddDays(7));
        }

        lastPayday = today.AddDays(7 - (_WeeklyPayday! - dayOfTheWeek));

        return new PayPeriod("", lastPayday!.Value, lastPayday.Value.AddDays(7));
    }

    #endregion
}