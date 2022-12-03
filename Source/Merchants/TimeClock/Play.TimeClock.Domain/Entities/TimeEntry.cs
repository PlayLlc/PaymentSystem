using Play.Core.Exceptions;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.TimeClock.Contracts.Dtos;
using Play.TimeClock.Domain.Aggregates;

namespace Play.TimeClock.Domain.Entities;

/// <summary>
///     A record of the hours that an <see cref="Employee" /> worked within a day
/// </summary>
public class TimeEntry : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _EmployeeId;

    private readonly DateTimeUtc _StartTime;
    private readonly DateTimeUtc _EndTime;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private TimeEntry()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal TimeEntry(TimeEntryDto dto)
    {
        Id = new SimpleStringId(dto.Id!);
        _EmployeeId = new SimpleStringId(dto.EmployeeId);

        if (dto.StartTime >= dto.EndTime)
            throw new ValueObjectException(
                $"The {nameof(TimeEntryDto.StartTime)} of the {nameof(TimeEntry)} must happen before the {nameof(TimeEntryDto.EndTime)}");

        try
        {
            _StartTime = new DateTimeUtc(dto.StartTime);
            _EndTime = new DateTimeUtc(dto.EndTime);
        }
        catch (PlayInternalException e)
        {
            throw new ValueObjectException($"The {nameof(_StartTime)} or {nameof(_EndTime)} provided was not in {nameof(DateTimeKind.Utc)} format", e);
        }
    }

    /// <exception cref="ValueObjectException"></exception>
    internal TimeEntry(string id, string employeeId, DateTimeUtc startTime, DateTimeUtc endTime)
    {
        Id = new SimpleStringId(id);
        _EmployeeId = new SimpleStringId(employeeId);

        if (startTime >= endTime)
            throw new ValueObjectException(
                $"The {nameof(TimeEntryDto.StartTime)} of the {nameof(TimeEntry)} must happen before the {nameof(TimeEntryDto.EndTime)}");

        _StartTime = startTime;
        _EndTime = endTime;
    }

    #endregion

    #region Instance Members

    public TimeSpan GetHoursWorked() => _StartTime - _EndTime;

    public override SimpleStringId GetId() => Id;

    public override TimeEntryDto AsDto() =>
        new TimeEntryDto
        {
            Id = Id,
            EmployeeId = _EmployeeId,
            StartTime = _StartTime,
            EndTime = _EndTime
        };

    #endregion
}