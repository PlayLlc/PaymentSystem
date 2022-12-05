﻿using Play.Domain.Common.ValueObjects;
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
    private bool IsTodayPaydayForBiweeklyPaySchedule(DateRange? lastPayPeriod)
    {
        ValidateBiweeklyPaySchedule();

        return DateTimeUtc.Now.AsShortDate() == GetNextBiweeklyPayPeriod(lastPayPeriod).GetExpirationDate().AsShortDate();
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

    /// <exception cref="ValueObjectException"></exception>
    private DateRange GetNextBiweeklyPayPeriod(DateRange? lastPayPeriod)
    {
        ValidateBiweeklyPaySchedule();
        DateTimeUtc now = DateTimeUtc.Now;

        if (lastPayPeriod is null)
            return new DateRange(now.GetLast(_WeeklyPayday!), now.GetNext(_WeeklyPayday!).AddDays(7));

        return new DateRange(lastPayPeriod!.Value.GetExpirationDate(), lastPayPeriod!.Value.GetExpirationDate().AddDays(14));
    }

    #endregion
}