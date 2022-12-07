using System;

using Play.Globalization.Time;

namespace Play.Events;

internal class EventEnvelope
{
    #region Instance Values

    private readonly EventHeader _EventHeader;
    private readonly Event _Event;

    #endregion

    #region Constructor

    public EventEnvelope(EventHeader eventHeader, Event @event)
    {
        _EventHeader = eventHeader;
        _Event = @event;
    }

    #endregion

    #region Instance Members

    public Event GetEvent() => _Event;
    public DateTimeUtc GetDateTimeUtc() => _EventHeader.DateTimeUtc;

    #endregion
}