using Play.Core.Exceptions;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Loyalty.Contracts.Dtosd;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public class TimeEntry : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _EmployeeId;
    private readonly DateTimeUtc _Start;
    private readonly DateTimeUtc _End;
    private readonly TimeEntryType _TimeEntryType;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private TimeEntry()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal TimeEntry(TimeEntryDto dto)
    {
        if (_End <= _Start)
            throw new ValueObjectException(
                $"The {nameof(TimeEntry)} cannot be initialized because the {nameof(dto.End)} argument provided does not happen after the {nameof(dto.Start)} argument provided;");

        Id = new SimpleStringId(dto.Id);
        _EmployeeId = new SimpleStringId(dto.EmployeeId);
        _TimeEntryType = new TimeEntryType(dto.TimeEntryType);

        try
        {
            _Start = new DateTimeUtc(dto.Start);
            _End = new DateTimeUtc(dto.End);
        }
        catch (PlayInternalException e)
        {
            throw new ValueObjectException($"The Start and End times provided must be in {nameof(DateTimeKind.Utc)} format", e);
        }
    }

    /// <exception cref="ValueObjectException"></exception>
    internal TimeEntry(string id, string employeeId, string timeEntryType, DateTimeUtc start, DateTimeUtc end)
    {
        if (_End <= _Start)
            throw new ValueObjectException(
                $"The {nameof(TimeEntry)} cannot be initialized because the {nameof(end)} argument provided did not happen after the {nameof(start)} argument provided;");

        Id = new SimpleStringId(id);
        _EmployeeId = new SimpleStringId(employeeId);
        _TimeEntryType = new TimeEntryType(timeEntryType);
        _Start = start;
        _End = end;
    }

    #endregion

    #region Instance Members

    internal DateTimeUtc GetStartTime() => _Start;
    internal DateTimeUtc GetEndTime() => _End;

    internal TimeEntryType GetTimeEntryType() => _TimeEntryType;

    public TimeSpan GetTimeWorked() => _End - _Start;

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