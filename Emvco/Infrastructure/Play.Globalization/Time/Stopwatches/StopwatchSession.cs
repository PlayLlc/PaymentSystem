using System;

using Play.Globalization.Time.Seconds;

namespace Play.Globalization.Time;

internal class StopwatchSession
{
    #region Instance Values

    private StopwatchInstance? _StopwatchBuddy;

    #endregion

    #region Constructor

    public StopwatchSession()
    {
        _StopwatchBuddy = null;
    }

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    public void Start()
    {
        if (_StopwatchBuddy != null)
        {
            throw new
                InvalidOperationException($"The {nameof(StopwatchSession)} could not be started because there is already a session running");
        }

        _StopwatchBuddy = new StopwatchInstance();
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Microseconds GetElapsedTime()
    {
        if (!IsRunning())
        {
            throw new
                InvalidOperationException($"The {nameof(StopwatchSession)} could not be stopped because there currently is not a session available");
        }

        return _StopwatchBuddy!.GetElapsedTime();
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Microseconds Stop()
    {
        if (!IsRunning())
        {
            throw new
                InvalidOperationException($"The {nameof(StopwatchSession)} could not be stopped because there currently is not a session available");
        }

        Microseconds elapsed = _StopwatchBuddy!.Stop();
        _StopwatchBuddy = null;

        return elapsed;
    }

    public bool IsRunning()
    {
        if (_StopwatchBuddy == null)
            return false;

        return true;
    }

    #endregion
}