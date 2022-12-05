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
    private bool IsTodaySemiMonthlyPayday()
    {
        DaysOfTheMonth dayOfTheMonth = DateTimeUtc.Now.GetDayOfTheMonth();

        return (_MonthlyPayday! == dayOfTheMonth) || (_SecondMonthlyPayday! == dayOfTheMonth);
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    private DateRange GetSemiMonthlyPayPeriod(ShortDate payday)
    {
        DaysOfTheMonth paydayDayOfTheMonth = payday.AsDateTimeUtc.GetDayOfTheMonth();

        if ((_MonthlyPayday! != payday.AsDateTimeUtc.GetDayOfTheMonth()) && (_SecondMonthlyPayday! != payday.AsDateTimeUtc.GetDayOfTheMonth()))
            throw new ValueObjectException(
                $"The {nameof(ShortDate)} provided is not a valid payday according to the {nameof(PaydaySchedule)} {nameof(PaydayRecurrence)}");

        if (paydayDayOfTheMonth == _MonthlyPayday!)
            return new DateRange(payday.AsDateTimeUtc.GetLast(_SecondMonthlyPayday!), payday);

        return new DateRange(payday.AsDateTimeUtc.GetLast(_MonthlyPayday!), payday);
    }

    /// <exception cref="ValueObjectException"></exception>
    private static PaydaySchedule CreateSemiMonthlySchedule(SimpleStringId id, DayOfTheMonth? monthlyPayday, DayOfTheMonth? secondMonthlyPayday)
    {
        if (monthlyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} could not be initialized. The {nameof(PaydayRecurrence)} type specified is {nameof(PaydayRecurrences.SemiMonthly)} but the {nameof(monthlyPayday)} argument is null");
        if (secondMonthlyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} could not be initialized. The {nameof(PaydayRecurrence)} type specified is {nameof(PaydayRecurrences.SemiMonthly)} but the {nameof(secondMonthlyPayday)} argument is null");

        return new PaydaySchedule(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.SemiMonthly), null, monthlyPayday, secondMonthlyPayday);
    }

    private DaysOfTheMonth GetLastPayday()
    {
        DateTimeUtc now = DateTimeUtc.Now;
        DateTimeUtc first = now.GetLast(_MonthlyPayday!);
        DateTimeUtc second = now.GetLast(_SecondMonthlyPayday!);

        if (now == first)
            return _MonthlyPayday!;
        if (now == second)
            return _SecondMonthlyPayday!;

        if (now < first)
            return _SecondMonthlyPayday!;

        if ((now < second) && (now > first))
            return _MonthlyPayday!;

        return _SecondMonthlyPayday!;
    }

    /// <exception cref="ValueObjectException"></exception>
    private void ValidateSemiMonthlyPaySchedule()
    {
        if (_WeeklyPayday is not null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.SemiMonthly)} {nameof(PaydayRecurrence)} operation but the {nameof(PaydaySchedule)} has an incorrect state. The  {nameof(_WeeklyPayday)} field MUST be null but is not;");

        if (_MonthlyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.SemiMonthly)} {nameof(PaydayRecurrence)} operation but the {nameof(_MonthlyPayday)} field is null. The {nameof(_MonthlyPayday)} field must not be null to perform {nameof(PaydayRecurrences.SemiMonthly)} operations;");

        if (_SecondMonthlyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} attempted a {nameof(PaydayRecurrences.SemiMonthly)} {nameof(PaydayRecurrence)} operation but the {nameof(_SecondMonthlyPayday)} field is null. The {nameof(_SecondMonthlyPayday)} field must not be null to perform {nameof(PaydayRecurrences.SemiMonthly)} operations;");
    }

    #endregion
}