using Microsoft.Extensions.Logging;

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
        _DirectDeposit = dto.DirectDeposit is null ? null : new DirectDeposit(dto.DirectDeposit!);
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

    internal bool AnyUndeliveredPaychecks() => _Paychecks.Any(a => !a.HasBeenDelivered());
    public IEnumerable<Paycheck> GetUndeliveredPaychecks() => _Paychecks.Where(a => !a.HasBeenDelivered());

    /// <exception cref="ValueObjectException"></exception>
    internal static Employee Create(string id, string userId, Compensation compensation) =>
        new(id, userId, compensation, Array.Empty<TimeEntry>(), Array.Empty<Paycheck>());

    /// <summary>
    ///     Distributes any undelivered paychecks to the employee's checking account specified in the
    ///     <see cref="DirectDeposit" /> field
    /// </summary>
    /// <param name="achClient"></param>
    /// <returns></returns>
    public async Task DisperseUndeliveredChecks(ISendAchTransfers achClient)
    {
        if (_DirectDeposit is null)
            return;

        List<Paycheck> undeliveredChecks = _Paychecks.Where(a => !a.HasBeenDelivered()).ToList();

        foreach (Paycheck check in undeliveredChecks)
        {
            if (!(await _DirectDeposit.SendPaycheck(achClient, check).ConfigureAwait(false)).Succeeded)
                continue;

            check.SetHasBeenDelivered();
        }
    }

    public void AddPaycheck(Paycheck paycheck)
    {
        _Paychecks.Add(paycheck);
    }

    internal IEnumerable<TimeEntry> GetTimeEntries(PayPeriod payPeriod) =>
        _TimeEntries.Where(a => (payPeriod.GetDateRange().GetStartDate() >= a.GetStartTime()) && (payPeriod.GetDateRange().GetEndDate() <= a.GetEndTime()));

    /// <exception cref="OverflowException"></exception>
    internal Money CalculatePaycheckEarnings(TimeSheet timeSheet) =>
        _Compensation.GetMinutelyWage((byte) timeSheet.GetPayPeriodEnd().Year) * timeSheet.GetBillableMinutes(_Compensation.GetCompensationType());

    public override SimpleStringId GetId() => Id;

    public override EmployeeDto AsDto() =>
        new()
        {
            Id = Id,
            UserId = _UserId,
            Compensation = _Compensation.AsDto(),
            DirectDeposit = _DirectDeposit?.AsDto(),
            TimeEntries = _TimeEntries.Select(a => a.AsDto()),
            Paychecks = _Paychecks.Select(a => a.AsDto())
        };

    #endregion
}