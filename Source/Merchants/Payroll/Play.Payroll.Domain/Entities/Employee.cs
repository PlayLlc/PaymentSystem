using Play.Core;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Domain.Entities;

public class Employee : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _UserId;
    private readonly Compensation _Compensation;
    private readonly DirectDeposit? _DirectDeposit;
    private readonly HashSet<TimeEntry> _TimeEntries;
    private readonly HashSet<Paycheck> _Paychecks;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private Employee()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Employee(EmployeeDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _UserId = new SimpleStringId(dto.UserId);
        _Compensation = new Compensation(dto.Compensation);
        _DirectDeposit = new DirectDeposit(dto.DirectDeposit);
        _TimeEntries = dto.TimeEntries.Select(a => new TimeEntry(a)).ToHashSet();
        _Paychecks = dto.Paychecks.Select(a => new Paycheck(a)).ToHashSet();
    }

    /// <exception cref="ValueObjectException"></exception>
    private Employee(
        string id, string userId, Compensation compensation, IEnumerable<TimeEntry> timeEntries, IEnumerable<Paycheck> paychecks,
        DirectDeposit? directDeposit = null)
    {
        Id = new SimpleStringId(id);
        _UserId = new SimpleStringId(userId);
        _Compensation = compensation;
        _TimeEntries = timeEntries.ToHashSet();
        _Paychecks = paychecks.ToHashSet();
        _DirectDeposit = directDeposit;
    }

    #endregion

    #region Instance Members

    internal string GetUserId() => _UserId;

    internal Paycheck? GetLatestPaycheck()
    {
        return _Paychecks.MaxBy(a => a.GetDateIssued());
    }

    public IEnumerable<Paycheck> GetUndeliveredPaychecks() => _Paychecks.Where(a => !a.HasBeenDistributed());

    /// <exception cref="ValueObjectException"></exception>
    internal static Employee Create(string id, string userId, Compensation compensation) =>
        new(id, userId, compensation, Array.Empty<TimeEntry>(), Array.Empty<Paycheck>());

    public async Task<Result> TryDispursingUndeliveredChecks(IISendAchTransfers achClient)
    {
        if (_DirectDeposit is null)
            return new Result($"Direct deposit has not been setup for the {nameof(Employee)} with the ID: [{_EmployeeId}]");

        List<Paycheck> undeliveredChecks = _Paychecks.Where(a => !a.HasBeenDistributed()).ToList();

        // HACK: Transactional Consistency!!!!!!!!!!
        // BUG Transactional Consistency
        // HACK: Transactional Consistency!!!!!!!!!!
        foreach (var check in undeliveredChecks)
            await _DirectDeposit.SendPaycheck(achClient, check).ConfigureAwait(false);

        return new Result();
    }

    public void AddPaycheck(Paycheck paycheck)
    {
        _Paychecks.Add(paycheck);
    }

    internal IEnumerable<TimeEntry> GetTimeEntries(PayPeriod payPeriod) =>
        _TimeEntries.Where(a => (payPeriod.Start >= a.GetStartTime()) && (payPeriod.End <= a.GetEndTime()));

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    private TimeSheet GenerateTimeSheet(SimpleStringId id, PayPeriod payPeriod)
    {
        var timeEntries = _TimeEntries.Where(a => (a.GetStartTime() >= payPeriod.Start) && (a.GetEndTime() <= payPeriod.End));

        return new TimeSheet(id, _EmployeeId, payPeriod, timeEntries);
    }

    // this can be used for not just generating paycheck -> sales associates view what they made so far
    internal Money CalculatePaycheckEarnings(TimeSheet timeSheet) =>
        _Compensation.GetMinutelyWage() * timeSheet.GetBillableMinutes(_Compensation.GetCompensationType());

    public override SimpleStringId GetId() => Id;

    public override EmployeeDto AsDto() =>
        new()
        {
            Id = Id,
            EmployeeId = _EmployeeId,
            Address = _Address.AsDto(),
            Compensation = _Compensation.AsDto(),
            DirectDeposit = _DirectDeposit.AsDto(),
            PaycheckHistory = _Paychecks.Select(a => a.AsDto()),
            TimeEntries = _TimeEntries.Select(a => a.AsDto())
        };

    #endregion
}