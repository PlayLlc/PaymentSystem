using Play.Core;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public partial class PaydaySchedule : Entity<SimpleStringId>
{
    #region Instance Member

    /// <exception cref="ValueObjectException"></exception>
    private static PaydaySchedule CreateWeeklySchedule(SimpleStringId id, DayOfTheWeek? weeklyPayday)
    {
        if (weeklyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} could not be initialized. The {nameof(PaydayRecurrence)} type specified is {nameof(PaydayRecurrences.Weekly)} but the {nameof(weeklyPayday)} argument is null");

        return new PaydaySchedule(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.Weekly), new DayOfTheWeek((byte) weeklyPayday), null, null);
    }

    /// <exception cref="ValueObjectException"></exception>
    private bool IsTodayWeeklyPayday() => _WeeklyPayday == DateTimeUtc.Now.GetDayOfTheWeek();

    /// <exception cref="ValueObjectException"></exception>
    private DateRange GetWeeklyPayPeriod(ShortDate payday)
    {
        if (_WeeklyPayday != payday.AsDateTimeUtc.GetDayOfTheWeek())
            throw new ValueObjectException(
                $"The {nameof(ShortDate)} provided is not a valid payday according to the {nameof(PaydaySchedule)} {nameof(PaydayRecurrence)}");

        return new DateRange(payday.AsDateTimeUtc.AddDays(-7), payday);
    }

    /// <exception cref="ValueObjectException"></exception>
    private DateRange GetNextWeeklyPayPeriod()
    {
        var nextPayday = DateTimeUtc.Now.GetNext(_WeeklyPayday!);
        var lastPayday = DateTimeUtc.Now.GetLast(_WeeklyPayday!);

        return new DateRange(lastPayday, nextPayday);
    }

    /// <exception cref="ValueObjectException"></exception>
    private void ValidateWeeklyPaySchedule()
    {
        if (_WeeklyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.Weekly)} {nameof(PaydayRecurrence)} operation but the {nameof(_WeeklyPayday)} field is null. The {nameof(_WeeklyPayday)} field must not be null to perform {nameof(PaydayRecurrences.Weekly)} operations;");

        if (_MonthlyPayday is not null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.Weekly)} {nameof(PaydayRecurrence)} operation but the {nameof(PaydaySchedule)} has an incorrect state. The  {nameof(_MonthlyPayday)} field MUST be null but is not;");

        if (_SecondMonthlyPayday is not null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.Weekly)} {nameof(PaydayRecurrence)} operation but the {nameof(PaydaySchedule)} has an incorrect state. The  {nameof(_SecondMonthlyPayday)} field MUST be null but is not;");
    }

    #endregion
}