using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public class Compensation : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly CompensationType _CompensationType;
    private readonly MoneyValueObject _HourlyWage;
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
        _HourlyWage = dto.HourlyWage.AsMoney();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Compensation(string id, string compensationType, Money hourlyWage)
    {
        Id = new SimpleStringId(id);
        _CompensationType = new CompensationType(compensationType);
        _HourlyWage = hourlyWage;
    }

    #endregion

    #region Instance Members

    public CompensationType GetCompensationType() => _CompensationType;
    public Money GetMinutelyWage() => new((ulong) ((Money) _HourlyWage / 60), GetNumericCurrencyCode());
    public NumericCurrencyCode GetNumericCurrencyCode() => _HourlyWage.NumericCurrencyCode;

    public override SimpleStringId GetId() => Id;

    public override CompensationDto AsDto() =>
        new()
        {
            Id = Id,
            CompensationType = _CompensationType,
            HourlyWage = _HourlyWage.AsDto()
        };

    #endregion
}