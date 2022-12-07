using Play.Core.Exceptions;
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
    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    private bool IsTodayBiweeklyPayday(DateRange? lastPayPeriod)
    {
        //if lastPayPeriod is null, we force the first payday to next week
        if (lastPayPeriod is null)
            return _WeeklyPayday == DateTimeUtc.Now.GetDayOfTheWeek();

        return lastPayPeriod.Value.GetEndDate().AddDays(14).AsShortDate() == DateTimeUtc.Now.AsShortDate();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private DateRange GetBiweeklyPayPeriod(ShortDate payday)
    {
        if (_WeeklyPayday != payday.AsDateTimeUtc.GetDayOfTheWeek())
            throw new ValueObjectException(
                $"The {nameof(ShortDate)} provided is not a valid payday according to the {nameof(PaydaySchedule)} {nameof(PaydayRecurrence)}");

        return new DateRange(payday.AsDateTimeUtc.AddDays(-14), payday);
    }

    /// <exception cref="ValueObjectException"></exception>
    private static PaydaySchedule CreateBiweeklySchedule(SimpleStringId id, DayOfTheWeek? weeklyPayday)
    {
        if (weeklyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} could not be initialized. The {nameof(PaydayRecurrence)} type specified is {nameof(PaydayRecurrences.Biweekly)} but the {nameof(weeklyPayday)} argument is null");

        return new PaydaySchedule(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.Biweekly), weeklyPayday, null, null);
    }

    /// <exception cref="ValueObjectException"></exception>
    private void ValidateBiweeklyPaySchedule()
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