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

    private readonly AsyncLocal<StopWatchInstance?> _TimeoutBuddy;

    #endregion

    #region Constructor

    public TimeoutSession()
    {
        _TimeoutBuddy = new AsyncLocal<StopWatchInstance?>();
    }

    #endregion

    #region Instance Members

    public void Start(Milliseconds timeout)
    {
        _TimeoutBuddy.Value = new StopWatchInstance(timeout);
    }

    public void Start(Milliseconds timeout, Action timeoutHandler)
    {
        Task.Run(() => { _TimeoutBuddy.Value = new StopWatchInstance(timeout); }).WithTimeout(timeout, () =>
        {
            _TimeoutBuddy.Value = null;
            timeoutHandler.Invoke();
        }).ConfigureAwait(false);
    }

    public void Stop()
    {
        _TimeoutBuddy.Value = null;
    }

    public bool IsRunning()
    {
        if (_TimeoutBuddy.Value == null)
            return false;

        return true;
    }

    #endregion
}