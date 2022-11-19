using System;

using Play.Globalization.Time;

namespace Play.Events;

internal record EventHeader
{
    #region Instance Values

    public readonly DateTimeUtc DateTimeUtc;

    #endregion

    #region Constructor

    protected EventHeader()
    {
        DateTimeUtc = new DateTimeUtc();
    }

    #endregion
}