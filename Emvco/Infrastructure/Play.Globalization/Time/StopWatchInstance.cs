using System;
using System.Diagnostics;

using Play.Globalization.Time.Seconds;

namespace Play.Globalization.Time;

/// <summary>
///     A transient object used to determine if a timeout has occurred
/// </summary>
internal class StopWatchInstance
{
    #region Instance Values

    private readonly Stopwatch _Stopwatch;
    private readonly Milliseconds _Timeout;

    #endregion

    #region Constructor

    public StopWatchInstance(Milliseconds timeout)
    {
        _Stopwatch = new Stopwatch();
        _Timeout = timeout;
        _Stopwatch.Start();
        _Stopwatch.Elapsed
    }

    #endregion

    #region Instance Members

    public Milliseconds Stop()
    {
        TimeSpan elapsed = _Stopwatch.Elapsed;
        _Stopwatch.Stop();

        return elapsed;
    }

    public Milliseconds GetElapsedTime() => _Stopwatch.Elapsed;
    public bool IsRunning() => (ulong) _Stopwatch.ElapsedMilliseconds < _Timeout;

    #endregion
}