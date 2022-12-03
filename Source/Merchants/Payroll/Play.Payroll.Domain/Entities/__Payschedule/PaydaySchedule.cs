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
    protected PaydaySchedule(
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

    public override PaydayScheduleDto AsDto() =>
        new PaydayScheduleDto()
        {
            Id = Id,
            PaydayRecurrence = _PaydayRecurrence,
            MonthlyPayday = _MonthlyPayday?.Value,
            SecondMonthlyPayday = _SecondMonthlyPayday?.Value,
            WeeklyPayday = _WeeklyPayday?.Value
        };

    internal PayPeriod GetNextPayPeriod()
    {
        if (_PaydayRecurrence == PaydayRecurrences.Weekly)
            return GetNextWeeklyPayPeriod();

        if (_PaydayRecurrence == PaydayRecurrences.Biweekly)
            return GetNextBiweeklyPayPeriod();

        if (_PaydayRecurrence == PaydayRecurrences.SemiMonthly)
            return GetNextBiweeklyPayPeriod();

        return GetNextMonthlyPayPeriod();
    }

    public override SimpleStringId GetId() => Id;

    #endregion
}