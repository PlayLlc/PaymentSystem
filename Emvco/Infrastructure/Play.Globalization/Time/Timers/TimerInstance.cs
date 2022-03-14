using System;
using System.Diagnostics;

using Play.Globalization.Time.Seconds;

namespace Play.Globalization.Time;

/// <summary>
///     A transient object used to determine if a timeout has occurred
/// </summary>
internal class TimerInstance
{
    #region Instance Values

    private readonly Stopwatch _Stopwatch;
    private readonly Milliseconds _Timeout;

    #endregion

    #region Constructor

    public TimerInstance(Milliseconds timeout)
    {
        _Stopwatch = new Stopwatch();
        _Timeout = timeout;
        _Stopwatch.Start();
    }

    #endregion

    #region Instance Members

    public Microseconds Stop()
    {
        TimeSpan elapsed = _Stopwatch.Elapsed;
        _Stopwatch.Stop();

        return elapsed;
    }

    public Microseconds GetElapsedTime() => _Stopwatch.Elapsed;

    #endregion
}