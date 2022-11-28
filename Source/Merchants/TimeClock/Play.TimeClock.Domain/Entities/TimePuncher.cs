using Play.Core.Exceptions;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.TimeClock.Contracts.Dtos;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Enums;
using Play.TimeClock.Domain.ValueObject;

namespace Play.TimeClock.Domain.Entities;

public class TimePuncher : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _EmployeeId;

    /// <summary>
    ///     The last four digits of the user's Social Security Number
    /// </summary>
    private TimeClockStatus _TimeClockStatus;

    private DateTimeUtc? _ClockedInAt;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private TimePuncher()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal TimePuncher(TimeClockDto dto)
    {
        Id = new SimpleStringId(dto.Id!);
        _EmployeeId = new SimpleStringId(dto.EmployeeId);
        _TimeClockStatus = new TimeClockStatus(dto.TimeClockStatus);

        try
        {
            _ClockedInAt = dto.ClockedInAt is null ? null : new DateTimeUtc(dto.ClockedInAt!.Value);
        }
        catch (PlayInternalException e)
        {
            throw new ValueObjectException($"The {nameof(TimeClockDto.ClockedInAt)} provided was not in {nameof(DateTimeKind.Utc)} format", e);
        }
    }

    /// <exception cref="ValueObjectException"></exception>
    internal TimePuncher(string id, string employeeId, string timeClockStatus, DateTimeUtc? dateTime)
    {
        Id = new SimpleStringId(id);
        _EmployeeId = new SimpleStringId(employeeId);
        _TimeClockStatus = new TimeClockStatus(timeClockStatus);
        _ClockedInAt = dateTime;
    }

    #endregion

    #region Instance Members

    internal TimeClockStatus GetTimeClockStatus() => _TimeClockStatus;

    /// <exception cref="ValueObjectException"></exception>
    public void ClockIn()
    {
        if (_TimeClockStatus == TimeClockStatuses.ClockedIn)
            throw new ValueObjectException(
                $"The {nameof(Employee)} could not {nameof(ClockIn)} because the {nameof(TimeClockStatus)} is already set to {nameof(TimeClockStatuses.ClockedIn)}");

        _ClockedInAt = DateTimeUtc.Now;
        _TimeClockStatus = TimeClockStatuses.ClockedIn;
    }

    /// <exception cref="ValueObjectException"></exception>
    public TimeEntry ClockOut(Func<string> timeEntryId)
    {
        if (_TimeClockStatus == TimeClockStatuses.ClockedOut)
            throw new ValueObjectException(
                $"The {nameof(Employee)} could not {nameof(ClockOut)} because the {nameof(TimeClockStatus)} is already set to {nameof(TimeClockStatuses.ClockedOut)}");

        DateTimeUtc clockedOutAt = DateTimeUtc.Now;

        return new TimeEntry(timeEntryId.Invoke(), _EmployeeId, _ClockedInAt!.Value, clockedOutAt);
    }

    public override SimpleStringId GetId() => Id;

    public override TimeClockDto AsDto() =>
        new()
        {
            Id = Id,
            TimeClockStatus = _TimeClockStatus,
            ClockedInAt = _ClockedInAt
        };

    #endregion
}