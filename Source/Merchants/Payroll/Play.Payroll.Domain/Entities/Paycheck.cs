using Play.Core.Exceptions;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Domain.Entities;

public class Paycheck : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _EmployeeId;
    private readonly MoneyValueObject _Amount;
    private readonly DateTimeUtc _DateIssued;
    private readonly TimeSheet _TimeSheet;
    private readonly DirectDeposit? _DirectDeposit;
    private readonly PayPeriod _PayPeriod;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private Paycheck()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Paycheck(PaycheckDto dto)
    {
        try
        {
            _DateIssued = new DateTimeUtc(dto.DateIssued);
        }
        catch (PlayInternalException e)
        {
            throw new ValueObjectException($"The {nameof(PaycheckDto.DateIssued)} provided must be in {nameof(DateTimeKind.Utc)} format", e);
        }

        Id = new SimpleStringId(dto.Id);
        _EmployeeId = new SimpleStringId(dto.EmployeeId);
        _Amount = dto.Amount;
        _TimeSheet = new TimeSheet(dto.TimeSheet);
        _DirectDeposit = dto?.DirectDeposit is null ? null : new DirectDeposit(dto.DirectDeposit);
        _PayPeriod = new PayPeriod(dto!.PayPeriod);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Paycheck(string id, string employeeId, Money amount, DateTimeUtc dateIssued, TimeSheet timeSheet, DirectDeposit directDeposit, PayPeriod payPeriod)
    {
        Id = new SimpleStringId(id);
        _EmployeeId = new SimpleStringId(employeeId);
        _Amount = amount;
        _DateIssued = dateIssued;
        _TimeSheet = timeSheet;
        _DirectDeposit = directDeposit;
        _DateIssued = dateIssued;
        _PayPeriod = payPeriod;
    }

    #endregion

    #region Instance Members

    public async Task<bool> TrySendingDirectDeposit(IISendAchTransfers achClient)
    {
        if (_DirectDeposit is null)
            return false;

        return await achClient.SendPaycheck(_EmployeeId, _DateIssued, _Amount, _DirectDeposit).ConfigureAwait(false);
    }

    public override SimpleStringId GetId() => Id;

    public override PaycheckDto AsDto() =>
        new()
        {
            Id = Id,
            EmployeeId = _EmployeeId,
            PayPeriod = _PayPeriod.AsDto(),
            Amount = _Amount,
            DateIssued = _DateIssued,
            DirectDeposit = _DirectDeposit?.AsDto(),
            TimeSheet = _TimeSheet.AsDto()
        };

    #endregion
}