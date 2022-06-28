using System;
using System.Diagnostics;

namespace Play.Globalization.Time;

internal class StopwatchInstance
{
    #region Instance Values

    private readonly Stopwatch _Stopwatch;

    #endregion

    #region Constructor

    public StopwatchInstance()
    {
        _Stopwatch = new Stopwatch();
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