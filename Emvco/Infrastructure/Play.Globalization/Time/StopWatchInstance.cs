using System.Diagnostics;

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
    }

    #endregion

    #region Instance Members

    public bool IsRunning() => (ulong) _Stopwatch.ElapsedMilliseconds < _Timeout;

    #endregion
}