using Play.Core.Exceptions;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Dtos;

namespace Play.Payroll.Domain.Entities;

public class PayPeriod : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly DateTimeUtc _Start;
    private readonly DateTimeUtc _End;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private PayPeriod()
    { }

    internal PayPeriod(string id, DateTimeUtc start, DateTimeUtc end)
    {
        Id = new SimpleStringId(id);
        _Start = start;
        _End = end;
    }

    internal PayPeriod(string id, DateRange range)
    {
        Id = new SimpleStringId(id);
        _Start = range.GetActivationDate();
        _End = range.GetExpirationDate();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal PayPeriod(PayPeriodDto dto)
    {
        if (_End <= _Start)
            throw new ValueObjectException(
                $"The {nameof(PayPeriod)} cannot be initialized because the {nameof(dto.End)} argument provided does not happen after the {nameof(dto.Start)} argument provided;");

        Id = new SimpleStringId(dto.Id);

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

    #endregion

    #region Instance Members

    internal DateRange GetDateRange() => new(_Start, _End);

    private static int GetWorkingDays(DateTime from, DateTime to)
    {
        int dayDifference = (int) to.Subtract(from).TotalDays;

        return Enumerable.Range(1, dayDifference)
            .Select(x => from.AddDays(x))
            .Count(x => (x.DayOfWeek != DayOfWeek.Saturday) && (x.DayOfWeek != DayOfWeek.Sunday));
    }

    public uint GetWeekdayWorkHours() => (uint) GetWorkingDays(_Start, _End) * 8;
    public uint GetWeekdayWorkMinutes() => GetWeekdayWorkHours() * 60;

    public bool IsTodayPayday() => _End.Day == DateTimeUtc.Now.Day;

    public override SimpleStringId GetId() => Id;

    public override PayPeriodDto AsDto() =>
        new()
        {
            Id = Id,
            Start = _Start,
            End = _End
        };

    #endregion
}