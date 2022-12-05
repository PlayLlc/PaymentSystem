using System;

using Play.Core.Exceptions;

namespace Play.Globalization.Time;

public readonly struct DateRange
{
    #region Instance Values

    private readonly DateTimeUtc _StartDate;
    private readonly DateTimeUtc _EndDate;

    #endregion

    #region Constructor

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public DateRange(ShortDate startDate, ShortDate endDate)
    {
        if (startDate > endDate)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(startDate),
                $"The argument {nameof(startDate)} is greater than the argument {nameof(endDate)}"));
        }

        _StartDate = startDate;
        _EndDate = endDate;
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public DateRange(DateTimeUtc startDate, DateTimeUtc endDate)
    {
        if (startDate > endDate)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(startDate),
                $"The argument {nameof(startDate)} is greater than the argument {nameof(endDate)}"));
        }

        _StartDate = startDate;
        _EndDate = endDate;
    }

    #endregion

    #region Instance Members

    public DateTimeUtc GetActivationDate() => _StartDate;
    public DateTimeUtc GetExpirationDate() => _EndDate;
    public bool IsActive() => (_EndDate > DateTimeUtc.Now) && (DateTimeUtc.Now > _StartDate);
    public bool IsExpired() => !IsActive();

    #endregion
}