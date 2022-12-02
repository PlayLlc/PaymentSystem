using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Dtos;

namespace Play.Payroll.Domain.Entities;

public class Employee : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _EmployeeId;
    private readonly Compensation _Compensation;
    private readonly DirectDeposit _DirectDeposit;
    private readonly Address _Address;
    private readonly HashSet<TimeEntry> _TimeEntries;
    private readonly HashSet<Paycheck> _PaycheckHistory;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private Employee()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Employee(TimeSheetDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _EmployeeId = new SimpleStringId(dto.EmployeeId);
        _Compensation = new Compensation(dto.Compensation);
        _DirectDeposit = new DirectDeposit(dto.DirectDeposit);
        _Address = new Address(dto.Address);
        _TimeEntries = dto.TimeEntries.Select(a => new TimeEntry(a)).ToHashSet();
        _PaycheckHistory = dto.PaycheckHistory.Select(a => new Paycheck(a)).ToHashSet();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Employee(
        string id, string employeeId, Compensation compensation, DirectDeposit directDeposit, Address address, IEnumerable<TimeEntry> timeEntries,
        IEnumerable<Paycheck> paychecks)
    {
        Id = new SimpleStringId(id);
        _EmployeeId = new SimpleStringId(employeeId);
        _Compensation = compensation;
        _DirectDeposit = directDeposit;
        _Address = address;
        _TimeEntries = timeEntries.ToHashSet();
        _PaycheckHistory = paychecks.ToHashSet();
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public Paycheck AddPaycheck(Func<SimpleStringId> generateSimpleStringId, PayPeriod payPeriod)
    {
        TimeSheet timeSheet = GenerateTimeSheet(generateSimpleStringId.Invoke(), payPeriod);
        Money paycheckAmount = CalculateEarnings(timeSheet);
        Paycheck paycheck = new(generateSimpleStringId.Invoke(), _EmployeeId, paycheckAmount, DateTimeUtc.Now, timeSheet, _DirectDeposit, payPeriod);
        _PaycheckHistory.Add(paycheck);

        return paycheck;
    }

    /// <exception cref="ValueObjectException"></exception>
    private TimeSheet GenerateTimeSheet(SimpleStringId id, PayPeriod payPeriod)
    {
        var timeEntries = _TimeEntries.Where(a => (a.GetStartTime() >= payPeriod.Start) && (a.GetEndTime() <= payPeriod.End));

        return new TimeSheet(id, _EmployeeId, payPeriod, timeEntries);
    }

    // this can be used for not just generating paycheck -> sales associates view what they made so far
    private Money CalculateEarnings(TimeSheet timeSheet) =>
        _Compensation.GetMinutelyWage() * timeSheet.GetTotalMinutesWorked(_Compensation.GetCompensationType());

    public override SimpleStringId GetId() => Id;

    public override TimeSheetDto AsDto() =>
        new()
        {
            Id = Id,
            EmployeeId = _EmployeeId,
            Address = _Address.AsDto(),
            Compensation = _Compensation.AsDto(),
            DirectDeposit = _DirectDeposit.AsDto(),
            PaycheckHistory = _PaycheckHistory.Select(a => a.AsDto()),
            TimeEntries = _TimeEntries.Select(a => a.AsDto())
        };

    #endregion
}