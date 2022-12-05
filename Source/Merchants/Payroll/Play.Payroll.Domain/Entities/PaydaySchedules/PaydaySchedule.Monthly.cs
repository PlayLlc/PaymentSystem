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
    private static PaydaySchedule CreateMonthlySchedule(SimpleStringId id, DayOfTheMonth? monthlyPayday)
    {
        if (monthlyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} could not be initialized. The {nameof(PaydayRecurrence)} type specified is {nameof(PaydayRecurrences.Monthly)} but the {nameof(monthlyPayday)} argument is null");

        return new PaydaySchedule(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.Monthly), null, monthlyPayday, null);
    }

    /// <exception cref="ValueObjectException"></exception>
    private bool IsTodayMonthlyPayday() => _MonthlyPayday! == DateTimeUtc.Now.GetDayOfTheMonth()!;

    /// <exception cref="ValueObjectException"></exception>
    private DateRange GetMonthlyPayPeriod(ShortDate payday)
    {
        DaysOfTheMonth paydayDayOfTheMonth = payday.AsDateTimeUtc.GetDayOfTheMonth();

        if (_MonthlyPayday! != paydayDayOfTheMonth)
            throw new ValueObjectException(
                $"The {nameof(ShortDate)} provided is not a valid payday according to the {nameof(PaydaySchedule)} {nameof(PaydayRecurrence)}");

        return new DateRange(payday.AsDateTimeUtc.GetLast(_MonthlyPayday!), payday);
    }

    /// <exception cref="ValueObjectException"></exception>
    private DateRange GetNextMonthlyPayPeriod()
    {
        DateTimeUtc now = DateTimeUtc.Now;

        return new DateRange(now.GetLast(_MonthlyPayday!), now.GetNext(_MonthlyPayday!));
    }

    /// <exception cref="ValueObjectException"></exception>
    private void ValidateMonthlyPaySchedule()
    {
        if (_WeeklyPayday is not null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.Monthly)} {nameof(PaydayRecurrence)} operation but the {nameof(PaydaySchedule)} has an incorrect state. The  {nameof(_WeeklyPayday)} field MUST be null but is not;");

        if (_MonthlyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.Monthly)} {nameof(PaydayRecurrence)} operation but the {nameof(_MonthlyPayday)} field is null. The {nameof(_MonthlyPayday)} field must not be null to perform {nameof(PaydayRecurrences.Monthly)} operations;");

        if (_SecondMonthlyPayday is not null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.Monthly)} {nameof(PaydayRecurrence)} operation but the {nameof(PaydaySchedule)} has an incorrect state. The  {nameof(_SecondMonthlyPayday)} field MUST be null but is not;");
    }

    #endregion
}