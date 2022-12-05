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
    private bool IsTodayPaydayForMonthlyPaySchedule()
    {
        ValidateMonthlyPaySchedule();

        return _MonthlyPayday! == DateTimeUtc.Now.GetDayOfTheMonth()!;
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

    /// <exception cref="ValueObjectException"></exception>
    private DateRange GetNextMonthlyPayPeriod()
    {
        ValidateMonthlyPaySchedule();

        DateTimeUtc now = DateTimeUtc.Now;

        return new DateRange(now.GetLast(_MonthlyPayday!), now.GetNext(_MonthlyPayday!));
    }

    #endregion
}