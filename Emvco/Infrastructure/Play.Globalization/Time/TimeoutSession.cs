using System;
using System.Threading;
using System.Threading.Tasks;

using Play.Core.Extensions.Tasks;

namespace Play.Globalization.Time;

/// <summary>
///     Manages the current session for the <see cref="TimeoutManager" />
/// </summary>
internal class TimeoutSession
{
    #region Instance Values

    private StopWatchInstance? _TimeoutBuddy;

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
            throw new
                InvalidOperationException($"The {nameof(TimeoutSession)} could not be started because there is already a session running");
        }

        _TimeoutBuddy = new StopWatchInstance(timeout);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public void Start(Milliseconds timeout, Action timeoutHandler)
    {
        if (_TimeoutBuddy != null)
        {
            throw new
                InvalidOperationException($"The {nameof(TimeoutSession)} could not be started because there is already a session running");
        }

        Task.Run(() => { _TimeoutBuddy = new StopWatchInstance(timeout); }).WithTimeout(timeout, () =>
        {
            _TimeoutBuddy = null;
            timeoutHandler.Invoke();
        }).ConfigureAwait(false);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Milliseconds GetElapsedTime()
    {
        if (!IsRunning())
        {
            throw new
                InvalidOperationException($"The {nameof(TimeoutSession)} could not be stopped because there currently is not a session available");
        }

        return _TimeoutBuddy!.GetElapsedTime();
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Milliseconds Stop()
    {
        if (!IsRunning())
        {
            throw new
                InvalidOperationException($"The {nameof(TimeoutSession)} could not be stopped because there currently is not a session available");
        }

        Milliseconds elapsed = _TimeoutBuddy!.Stop();
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