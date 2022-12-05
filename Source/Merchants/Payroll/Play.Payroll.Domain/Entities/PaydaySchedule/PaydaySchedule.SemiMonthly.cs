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

    /// <exception cref="ValueObjectException"></exception>
    private bool IsTodayPaydayForSemiMonthlyPaySchedule()
    {
        ValidateSemiMonthlyPaySchedule();

        var dayOfTheMonth = DateTimeUtc.Now.GetDayOfTheMonth();

        return (_MonthlyPayday! == dayOfTheMonth) || (_SecondMonthlyPayday! == dayOfTheMonth);
    }

    /// <exception cref="ValueObjectException"></exception>
    private DateRange GetNextSemiMonthlyPayPeriod()
    {
        ValidateSemiMonthlyPaySchedule();
        DaysOfTheMonth lastPayday = GetLastPayday();
        DateTimeUtc now = DateTimeUtc.Now;

        return lastPayday == _MonthlyPayday!
            ? new DateRange(now.GetLast(_MonthlyPayday!), now.GetNext(_SecondMonthlyPayday!))
            : new DateRange(now.GetLast(_SecondMonthlyPayday!), now.GetNext(_MonthlyPayday!));
    }

    private DaysOfTheMonth GetLastPayday()
    {
        var now = DateTimeUtc.Now;
        var first = now.GetLast(_MonthlyPayday!);
        var second = now.GetLast(_SecondMonthlyPayday!);

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

    #endregion
}