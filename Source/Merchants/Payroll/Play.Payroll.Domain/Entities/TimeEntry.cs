using Play.Core.Exceptions;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public class TimeEntry : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _EmployeeId;
    private DateTimeUtc _Start;
    private DateTimeUtc _End;
    private TimeEntryType _TimeEntryType;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private TimeEntry()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal TimeEntry(TimeEntryDto dto)
    {
        if (dto.End <= dto.Start)
            throw new ValueObjectException(
                $"The {nameof(TimeEntry)} cannot be initialized because the {nameof(dto.End)} argument provided does not happen after the {nameof(dto.Start)} argument provided;");

        Id = new(dto.Id);
        _EmployeeId = new(dto.EmployeeId);
        _TimeEntryType = new(dto.TimeEntryType);

        try
        {
            _Start = new(dto.Start);
            _End = new(dto.End);
        }
        catch (PlayInternalException e)
        {
            throw new ValueObjectException($"The Start and End times provided must be in {nameof(DateTimeKind.Utc)} format", e);
        }
    }

    /// <exception cref="ValueObjectException"></exception>
    internal TimeEntry(string id, string employeeId, string timeEntryType, DateTimeUtc start, DateTimeUtc end)
    {
        if (end <= start)
            throw new ValueObjectException(
                $"The {nameof(TimeEntry)} cannot be initialized because the {nameof(end)} argument provided did not happen after the {nameof(start)} argument provided;");

        Id = new(id);
        _EmployeeId = new(employeeId);
        _TimeEntryType = new(timeEntryType);
        _Start = start;
        _End = end;
    }

    #endregion

    #region Instance Members

    public DateTimeUtc GetStartTime() => _Start;
    public DateTimeUtc GetEndTime() => _End;

    /// <exception cref="ValueObjectException"></exception>
    public void Update(string timeEntryType, DateTimeUtc start, DateTimeUtc end)
    {
        if (end <= start)
            throw new ValueObjectException(
                $"The {nameof(TimeEntry)} cannot be initialized because the {nameof(end)} argument provided did not happen after the {nameof(start)} argument provided;");

        _TimeEntryType = new(timeEntryType);
        _Start = start;
        _End = end;
    }

    internal TimeEntryType GetTimeEntryType() => _TimeEntryType;

    public TimeSpan GetBillableHours() => _End - _Start;

    public override SimpleStringId GetId() => Id;

    public override TimeEntryDto AsDto() =>
        new()
        {
            Id = Id,
            EmployeeId = _EmployeeId,
            End = _End,
            Start = _Start,
            TimeEntryType = _TimeEntryType
        };

    #endregion
}