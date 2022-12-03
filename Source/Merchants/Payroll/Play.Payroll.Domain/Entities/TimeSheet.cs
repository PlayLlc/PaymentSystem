using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public class TimeSheet : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _EmployeeId;
    private readonly HashSet<TimeEntry> _TimeEntries;
    private readonly PayPeriod _PayPeriod;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private TimeSheet()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal TimeSheet(TimeSheetDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _EmployeeId = new SimpleStringId(dto.EmployeeId);
        _PayPeriod = new PayPeriod(dto.PayPeriod);
        _TimeEntries = dto.TimeEntries.Select(a => new TimeEntry(a)).ToHashSet();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal TimeSheet(string id, string employeeId, PayPeriod payPeriod, IEnumerable<TimeEntry> timeEntries)
    {
        Id = new SimpleStringId(id);
        _EmployeeId = new SimpleStringId(employeeId);
        _PayPeriod = payPeriod;
        _TimeEntries = timeEntries.ToHashSet();
    }

    #endregion

    #region Instance Members

    internal DateTimeUtc GetPayPeriodStart() => _PayPeriod.Start;
    internal DateTimeUtc GetPayPeriodEnd() => _PayPeriod.End;

    /// <summary>
    ///     Gets the amount of minutes that an employee worked within a pay period
    /// </summary>
    /// <returns></returns>
    public uint GetBillableMinutes(CompensationType compensationType) =>
        compensationType == CompensationTypes.Hourly ? GetMinutesClockedInForHourlyEmployee() : GetMinutesClockedInForSalariedEmployee();

    private uint GetMinutesClockedInForSalariedEmployee()
    {
        uint workableMinutes = _PayPeriod.GetWeekdayWorkMinutes();
        TimeSpan unpaidTime = new TimeSpan();

        foreach (var timeEntry in _TimeEntries.Where(a => (_PayPeriod.Start <= a.GetStart()) && (_PayPeriod.End >= a.GetEnd())))
        {
            if (timeEntry.GetTimeEntryType() != TimeEntryTypes.UnpaidTime)
                continue;

            unpaidTime += timeEntry.GetHoursBilled();
        }

        return (uint) (workableMinutes - unpaidTime.Minutes);
    }

    private uint GetMinutesClockedInForHourlyEmployee()
    {
        TimeSpan timeWorked = new TimeSpan();

        foreach (var timeEntry in _TimeEntries.Where(a => (_PayPeriod.Start <= a.GetStart()) && (_PayPeriod.End >= a.GetEnd())))
        {
            if (timeEntry.GetTimeEntryType() == TimeEntryTypes.UnpaidTime)
                continue;

            timeWorked += timeEntry.GetHoursBilled();
        }

        return (uint) timeWorked.Minutes;
    }

    internal IEnumerable<TimeEntry> GeTimeEntries() => _TimeEntries;

    public override SimpleStringId GetId() => Id;

    public override TimeSheetDto AsDto() =>
        new TimeSheetDto
        {
            Id = Id,
            EmployeeId = _EmployeeId,
            PayPeriod = _PayPeriod.AsDto(),
            TimeEntries = _TimeEntries.Select(a => a.AsDto())
        };

    #endregion
}