using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Extensions;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public class Compensation : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly CompensationType _CompensationType;
    private readonly MoneyValueObject _CompensationRate;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private Compensation()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Compensation(CompensationDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _CompensationType = new CompensationType(dto.CompensationType);
        _CompensationRate = dto.HourlyWage.AsMoney();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Compensation(string id, string compensationType, Money compensationRate)
    {
        Id = new SimpleStringId(id);
        _CompensationType = new CompensationType(compensationType);
        _CompensationRate = compensationRate;
    }

    #endregion

    #region Instance Members

    public static Compensation Create(string id, CompensationTypes compensationType, Money compensationRate) =>
        new Compensation(new string(id), compensationType, compensationRate);

    /// <exception cref="OverflowException"></exception>
    public Money GetPaycheckTotal(TimeSheet timeSheet)
    {
        if (_CompensationType == CompensationTypes.Hourly)
            return CalculateHourlyPaycheck(timeSheet);

        return CalculateSalariedPaycheck(timeSheet);
    }

    /// <exception cref="OverflowException"></exception>
    private Money CalculateSalariedPaycheck(TimeSheet timeSheet) =>
        GetSalaryEmployeesWagePerMinute(timeSheet) * (ulong) timeSheet.GetBillableMinutes(_CompensationType);

    private Money CalculateHourlyPaycheck(TimeSheet timeSheet)
    {
        // HACK: Check we're not fucking the worker here
        Money minutelyWage = new Money((ulong) (_CompensationRate.AsMoney() / 60), _CompensationRate.NumericCurrencyCode);

        return minutelyWage * timeSheet.GetBillableMinutes(_CompensationType);
    }

    public CompensationType GetCompensationType() => _CompensationType;
    public Money GetMinutelyWage() => new Money((ulong) ((Money) _CompensationRate / 60), GetNumericCurrencyCode());
    public NumericCurrencyCode GetNumericCurrencyCode() => _CompensationRate.NumericCurrencyCode;

    /// <exception cref="OverflowException"></exception>
    private Money GetSalaryEmployeesWagePerMinute(TimeSheet timeSheet)
    {
        // BUG: This doesn't compensate for when part of the pay period is in one year and the remainder is in the new year
        int year = timeSheet.GetPayPeriodStart().Year;

        DateTimeUtc start = new DateTimeUtc(new DateTime(year, 1, 1));
        DateTimeUtc end = new DateTimeUtc(new DateTime(year + 1, 1, 1).Subtract(new TimeSpan(0, 0, 1, 0)));

        int businessDays = start.GetBusinessDays(end);
        ulong minutelyWage = (ulong) (_CompensationRate.AsMoney() / (uint) (businessDays * 8 * 60));

        return new Money(minutelyWage, _CompensationRate.NumericCurrencyCode);
    }

    public override SimpleStringId GetId() => Id;

    public override CompensationDto AsDto() =>
        new CompensationDto
        {
            Id = Id,
            CompensationType = _CompensationType,
            HourlyWage = _CompensationRate.AsDto()
        };

    #endregion
}