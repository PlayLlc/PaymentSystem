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

    public void Start(Milliseconds timeout)
    {
        _TimeoutBuddy = new StopWatchInstance(timeout);
    }

    public void Stop()
    {
        _TimeoutBuddy = null;
    }

    public bool IsRunning()
    {
        if (_TimeoutBuddy == null)
            return false;

        if (!_TimeoutBuddy.IsRunning())
        {
            _TimeoutBuddy = null;

            return false;
        }

        return true;
    }

    #endregion
}