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
    internal PaydaySchedule()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal PaydaySchedule(
        SimpleStringId id, PaydayRecurrence paydayRecurrence, DayOfTheWeek? weeklyPayday, DayOfTheMonth? monthlyPayday, DayOfTheMonth? secondMonthlyPayday)
    {
        Id = id;
        _PaydayRecurrence = paydayRecurrence;
        _WeeklyPayday = weeklyPayday;
        _MonthlyPayday = monthlyPayday;
        _SecondMonthlyPayday = secondMonthlyPayday;
        ValidatePaydaySchedule();
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public static PaydaySchedule Create(
        string id, PaydayRecurrence paydayRecurrence, DayOfTheWeek? weeklyPayday, DayOfTheMonth? monthlyPayday, DayOfTheMonth? secondMonthlyPayday)
    {
        if (paydayRecurrence == PaydayRecurrences.Weekly)
            return CreateWeeklySchedule(new SimpleStringId(id), weeklyPayday);

        if (paydayRecurrence == PaydayRecurrences.Biweekly)
            return CreateBiweeklySchedule(new SimpleStringId(id), weeklyPayday);

        if (paydayRecurrence == PaydayRecurrences.SemiMonthly)
            return CreateSemiMonthlySchedule(new SimpleStringId(id), monthlyPayday, secondMonthlyPayday);

        if (paydayRecurrence == PaydayRecurrences.Monthly)
            return CreateMonthlySchedule(new SimpleStringId(id), monthlyPayday);

        throw new ValueObjectException($"The {nameof(PaydayRecurrence)} with the value: [{paydayRecurrence}] was not recognized");
    }

    /// <exception cref="ValueObjectException"></exception>
    internal bool IsTodayPayday(DateRange? lastPayPeriod)
    {
        ValidatePaydaySchedule();

        if (_PaydayRecurrence == PaydayRecurrences.Weekly)
            return IsTodayWeeklyPayday();

        if (_PaydayRecurrence == PaydayRecurrences.Biweekly)
            return IsTodayBiweeklyPayday(lastPayPeriod);

        if (_PaydayRecurrence == PaydayRecurrences.SemiMonthly)
            return IsTodaySemiMonthlyPayday();

        if (_PaydayRecurrence == PaydayRecurrences.Monthly)
            return IsTodayMonthlyPayday();

        throw new ValueObjectException($"The {nameof(_PaydayRecurrence)} value was not recognized");
    }

    /// <exception cref="ValueObjectException"></exception>
    internal DateRange GetNextPayPeriod(DateRange? lastPayPeriod)
    {
        ValidateWeeklyPaySchedule();

        if (_PaydayRecurrence == PaydayRecurrences.Weekly)
            return GetNextWeeklyPayPeriod();

        if (_PaydayRecurrence == PaydayRecurrences.Biweekly)
            return GetNextBiweeklyPayPeriod(lastPayPeriod);

        if (_PaydayRecurrence == PaydayRecurrences.SemiMonthly)
            return GetNextSemiMonthlyPayPeriod();

        if (_PaydayRecurrence == PaydayRecurrences.Monthly)
            return GetNextMonthlyPayPeriod();

        throw new ValueObjectException($"The {nameof(_PaydayRecurrence)} value was not recognized");
    }

    /// <exception cref="ValueObjectException"></exception>
    private void ValidatePaydaySchedule()
    {
        if (_PaydayRecurrence == PaydayRecurrences.Weekly)
            ValidateWeeklyPaySchedule();

        if (_PaydayRecurrence == PaydayRecurrences.Biweekly)
            ValidateBiweeklyPaySchedule();

        if (_PaydayRecurrence == PaydayRecurrences.SemiMonthly)
            ValidateSemiMonthlyPaySchedule();

        if (_PaydayRecurrence == PaydayRecurrences.Monthly)
            ValidateMonthlyPaySchedule();

        throw new ValueObjectException($"The {nameof(_PaydayRecurrence)} value was not recognized");
    }

    public override SimpleStringId GetId() => Id;

    public override PaydayScheduleDto AsDto() =>
        new()
        {
            Id = Id,
            PaydayRecurrence = _PaydayRecurrence,
            MonthlyPayday = _MonthlyPayday?.Value,
            SecondMonthlyPayday = _SecondMonthlyPayday?.Value,
            WeeklyPayday = _WeeklyPayday?.Value
        };

    #endregion
}