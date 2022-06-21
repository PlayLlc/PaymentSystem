using System;

namespace Play.Globalization.Time;

public class StopwatchManager
{
    #region Instance Values

    private readonly StopwatchSession _StopwatchSession;

    #endregion

    #region Constructor

    public StopwatchManager()
    {
        _StopwatchSession = new StopwatchSession();
    }

    #endregion

    #region Instance Members

    #region Stop

    /// <summary>
    ///     Stop
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public Microseconds Stop()
    {
        // TODO: Do we really need to be locking in this item? I don't think so. Make sure we don't need to be thread safe
        lock (_StopwatchSession)
        {
            return _StopwatchSession.Stop();
        }
    }

    #endregion

    #endregion

    #region Start

    /// <summary>
    ///     Starts a new Timeout Session and invokes the timeoutHandler if the session times out
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void Start()
    {
        lock (_StopwatchSession)
        {
            if (_StopwatchSession.IsRunning())
            {
                throw new InvalidOperationException(
                    $"The {nameof(StopwatchManager)} could not complete the {nameof(Start)} method because the {nameof(StopwatchManager)} is currently running");
            }

            _StopwatchSession.Start();
        }
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Microseconds GetElapsedTime()
    {
        lock (_StopwatchSession)
        {
            if (!_StopwatchSession.IsRunning())
            {
                throw new InvalidOperationException(
                    $"The {nameof(StopwatchManager)} could not complete the {nameof(GetElapsedTime)} method because the {nameof(StopwatchManager)} is currently not running");
            }

            return _StopwatchSession.GetElapsedTime();
        }
    }

    #endregion
}