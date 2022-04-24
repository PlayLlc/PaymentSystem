using System;

namespace Play.Globalization.Time;

public readonly struct DateRange
{
    #region Instance Values

    private readonly ShortDate _ActivationDate;
    private readonly ShortDate _ExpiryDate;

    #endregion

    #region Constructor

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public DateRange(ShortDate activationDate, ShortDate expiryDate)
    {
        if (activationDate > expiryDate)
        {
            throw new ArgumentOutOfRangeException(nameof(activationDate),
                $"The argument {nameof(activationDate)} is greater than the argument {nameof(expiryDate)}");
        }

        _ActivationDate = activationDate;
        _ExpiryDate = expiryDate;
    }

    #endregion

    #region Instance Members

    public ShortDate GetActivationDate() => _ActivationDate;
    public ShortDate GetExpirationDate() => _ExpiryDate;
    public bool IsActive() => _ExpiryDate < DateTimeUtc.Now;
    public bool IsExpired() => !IsActive();

    #endregion
}