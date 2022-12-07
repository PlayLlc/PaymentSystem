using Play.Core.Exceptions;
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
    private MoneyValueObject _CompensationRate;
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

    public void Update(CompensationType compensationType, MoneyValueObject compensationRate)
    {
        _CompensationRate = compensationRate;
        _CompensationRate = compensationRate;
    }

    public static Compensation Create(string id, CompensationTypes compensationType, Money compensationRate) =>
        new(new string(id), compensationType, compensationRate);

    public CompensationType GetCompensationType() => _CompensationType;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    public Money GetMinutelyWage(byte year) =>
        _CompensationType == CompensationTypes.Salary
            ? GetSalaryEmployeesWagePerMinute(year)
            : new Money((ulong) (_CompensationRate.AsMoney() / (byte) 60), GetNumericCurrencyCode());

    public NumericCurrencyCode GetNumericCurrencyCode() => _CompensationRate.NumericCurrencyCode;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private Money GetSalaryEmployeesWagePerMinute(byte year)
    {
        // BUG: This doesn't compensate for when part of the pay period is in one year and the remainder is in the new year

        DateTimeUtc start = new(new DateTime(year, 1, 1));
        DateTimeUtc end = new(new DateTime(year + 1, 1, 1).Subtract(new TimeSpan(0, 0, 1, 0)));

        int businessDays = start.GetBusinessDays(end);
        ulong minutelyWage = (ulong) (_CompensationRate.AsMoney() / (uint) (businessDays * 8 * 60));

        return new Money(minutelyWage, _CompensationRate.NumericCurrencyCode);
    }

    public override SimpleStringId GetId() => Id;

    public override CompensationDto AsDto() =>
        new()
        {
            Id = Id,
            CompensationType = _CompensationType,
            HourlyWage = _CompensationRate.AsDto()
        };

    #endregion
}