using Play.Domain;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public partial class PaydaySchedule : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly PaydayRecurrence _PaydayRecurrence;

    private readonly DayOfTheWeek? _WeeklyPayday;
    private readonly DayOfTheMonth? _MonthlyPayday;
    private readonly DayOfTheMonth? _SecondMonthlyPayday;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    internal PaydaySchedule(
        SimpleStringId id, PaydayRecurrence paydayRecurrence, DayOfTheWeek? weeklyPayday, DayOfTheMonth? monthlyPayday, DayOfTheMonth? secondMonthlyPayday)
    {
        Id = id;
        _PaydayRecurrence = paydayRecurrence;
        _WeeklyPayday = weeklyPayday;
        _MonthlyPayday = monthlyPayday;
        _SecondMonthlyPayday = secondMonthlyPayday;
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    internal bool IsTodayPayday(DateRange? lastPayPeriod)
    {
        if (_PaydayRecurrence == PaydayRecurrences.Weekly)
            return IsTodayPaydayForWeeklyPaySchedule();

        if (_PaydayRecurrence == PaydayRecurrences.Biweekly)
            return IsTodayPaydayForBiweeklyPaySchedule(lastPayPeriod);

        if (_PaydayRecurrence == PaydayRecurrences.SemiMonthly)
            return IsTodayPaydayForSemiMonthlyPaySchedule();

        if (_PaydayRecurrence == PaydayRecurrences.Monthly)
            return IsTodayPaydayForMonthlyPaySchedule();

        throw new ValueObjectException($"The {nameof(_PaydayRecurrence)} value was not recognized");
    }

    public override PaydayScheduleDto AsDto() =>
        new()
        {
            Id = Id,
            PaydayRecurrence = _PaydayRecurrence,
            MonthlyPayday = _MonthlyPayday?.Value,
            SecondMonthlyPayday = _SecondMonthlyPayday?.Value,
            WeeklyPayday = _WeeklyPayday?.Value
        };

    /// <exception cref="ValueObjectException"></exception>
    internal DateRange GetNextPayPeriod(DateRange? lastPayPeriod)
    {
        if (_PaydayRecurrence == PaydayRecurrences.Weekly)
            return GetNextWeeklyPayPeriod();

        if (_PaydayRecurrence == PaydayRecurrences.Biweekly)
            return GetNextBiweeklyPayPeriod(lastPayPeriod);

        if (_PaydayRecurrence == PaydayRecurrences.SemiMonthly)
            return GetNextSemiMonthlyPayPeriod();

        return GetNextMonthlyPayPeriod();
    }

    public override SimpleStringId GetId() => Id;

    #endregion

    #region Static Factory

    /// <exception cref="ValueObjectException"></exception>
    public static PaydaySchedule Create(
        string id, PaydayRecurrence paydayRecurrence, DayOfTheWeek? weeklyPayday, DayOfTheMonth? monthlyPayday, DayOfTheMonth? secondMonthlyPayday)
    {
        if (paydayRecurrence == PaydayRecurrences.Weekly)
        {
            var weeklyPaySchedule = CreateWeeklySchedule(new SimpleStringId(id), weeklyPayday);
            weeklyPaySchedule.ValidateWeeklyPaySchedule();
        }

        if (paydayRecurrence == PaydayRecurrences.Biweekly)
        {
            var biweeklyPaySchedule = CreateBiweeklySchedule(new SimpleStringId(id), weeklyPayday);
            biweeklyPaySchedule.ValidateBiweeklyPaySchedule();
        }

        if (paydayRecurrence == PaydayRecurrences.SemiMonthly)
        {
            var semiMonthlyPaySchedule = CreateSemiMonthlySchedule(new SimpleStringId(id), monthlyPayday, secondMonthlyPayday);
            semiMonthlyPaySchedule.ValidateSemiMonthlyPaySchedule();
        }

        var monthlyPaySchedule = CreateMonthlySchedule(new SimpleStringId(id), monthlyPayday);
        monthlyPaySchedule.ValidateMonthlyPaySchedule();

        return monthlyPaySchedule;
    }

    /// <exception cref="ValueObjectException"></exception>
    private static PaydaySchedule CreateWeeklySchedule(SimpleStringId id, DayOfTheWeek? weeklyPayday)
    {
        if (weeklyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} could not be initialized. The {nameof(PaydayRecurrence)} type specified is {nameof(PaydayRecurrences.Weekly)} but the {nameof(weeklyPayday)} argument is null");

        return new PaydaySchedule(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.Weekly), new DayOfTheWeek((byte) weeklyPayday), null, null);
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

    /// <exception cref="ValueObjectException"></exception>
    private static PaydaySchedule CreateMonthlySchedule(SimpleStringId id, DayOfTheMonth? monthlyPayday)
    {
        if (monthlyPayday is null)
            throw new ValueObjectException(
                $"The {nameof(PaydaySchedule)} could not be initialized. The {nameof(PaydayRecurrence)} type specified is {nameof(PaydayRecurrences.Monthly)} but the {nameof(monthlyPayday)} argument is null");

        return new PaydaySchedule(new SimpleStringId(id), new PaydayRecurrence(PaydayRecurrences.Monthly), null, monthlyPayday, null);
    }

    #endregion
}