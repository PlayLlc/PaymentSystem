using System;

namespace Play.Globalization.Time;

public readonly struct DateRange
{
    #region Instance Values

    private readonly ShortDateValue _ActivationDate;
    private readonly ShortDateValue _ExpiryDate;

    #endregion

    #region Constructor

    public DateRange(ShortDateValue activationDate, ShortDateValue expiryDate)
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

    public ShortDateValue GetActivationDate() => _ActivationDate;
    public ShortDateValue GetExpirationDate() => _ExpiryDate;
    public bool IsActive() => _ExpiryDate < DateTimeUtc.Now();
    public bool IsExpired() => !IsActive();

    #endregion
}