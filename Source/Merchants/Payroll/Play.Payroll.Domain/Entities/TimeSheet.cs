using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
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
        // TODO: Check that all time entriese are within pay period
        // if(_TimeEntries.Any > _payPeriod...)

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

    internal IEnumerable<TimeEntry> GeTimeEntries() => _TimeEntries;

    public uint GetTotalMinutesWorked(CompensationType compensationType)
    {
        if (compensationType == CompensationTypes.Salary)
        {
            uint workHours = _PayPeriod.GetWeekdayWorkMinutes();

            foreach (var a in _TimeEntries)
                workHours -= (uint) (a.GetEndTime() - a.GetStartTime()).Minutes;

            return workHours;
        }

        return (uint) _TimeEntries.Sum(a => a.GetTimeWorked().Minutes);
    }

    public override SimpleStringId GetId() => Id;

    public override TimeSheetDto AsDto() =>
        new()
        {
            Id = Id,
            EmployeeId = _EmployeeId,
            PayPeriod = _PayPeriod.AsDto(),
            TimeEntries = _TimeEntries.Select(a => a.AsDto())
        };

    #endregion
}