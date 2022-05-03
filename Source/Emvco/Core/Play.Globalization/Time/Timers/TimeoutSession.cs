using System;
using System.Threading.Tasks;

using Play.Core.Extensions.Tasks;
using Play.Globalization.Time.Seconds;

namespace Play.Globalization.Time;

/// <summary>
///     Manages the current session for the <see cref="TimeoutManager" />
/// </summary>
internal class TimeoutSession
{
    #region Instance Values

    private TimerInstance? _TimeoutBuddy;

    #endregion

    #region Constructor

    public TimeoutSession()
    {
        _TimeoutBuddy = null;
    }

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    public void Start(Milliseconds timeout)
    {
        if (_TimeoutBuddy != null)
        {
            throw new InvalidOperationException($"The {nameof(TimeoutSession)} could not be started because there is already a session running");
        }

        _TimeoutBuddy = new TimerInstance(timeout);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public void Start(Milliseconds timeout, Action timeoutHandler)
    {
        if (_TimeoutBuddy != null)
        {
            throw new InvalidOperationException($"The {nameof(TimeoutSession)} could not be started because there is already a session running");
        }

        Task.Run(() => { _TimeoutBuddy = new TimerInstance(timeout); }).WithTimeout(timeout, () =>
        {
            _TimeoutBuddy = null;
            timeoutHandler.Invoke();
        }).ConfigureAwait(false);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Microseconds GetElapsedTime()
    {
        if (!IsRunning())
        {
            throw new InvalidOperationException($"The {nameof(TimeoutSession)} could not be stopped because there currently is not a session available");
        }

        return _TimeoutBuddy!.GetElapsedTime();
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Microseconds Stop()
    {
        if (!IsRunning())
        {
            throw new InvalidOperationException($"The {nameof(TimeoutSession)} could not be stopped because there currently is not a session available");
        }

        Microseconds elapsed = _TimeoutBuddy!.Stop();
        _TimeoutBuddy = null;

        return elapsed;
    }

    public bool IsRunning()
    {
        if (_TimeoutBuddy == null)
            return false;

        return true;
    }

    #endregion
}