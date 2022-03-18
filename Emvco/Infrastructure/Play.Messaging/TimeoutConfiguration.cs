using Play.Globalization.Time.Seconds;

namespace Play.Messaging;

public record TimeoutConfiguration
{
    #region Instance Values

    private readonly Seconds _Seconds;
    private readonly Action _TimeoutHandler;

    #endregion

    #region Constructor

    public TimeoutConfiguration(Milliseconds seconds, Action timeoutHandler)
    {
        _Seconds = seconds;
        _TimeoutHandler = timeoutHandler;
    }

    public TimeoutConfiguration(Milliseconds seconds)
    {
        _Seconds = seconds;
        _TimeoutHandler = () => { };
    }

    #endregion

    #region Instance Members

    public Seconds GetTimeout() => _Seconds;
    public Action GetTimeoutHandler() => _TimeoutHandler;

    #endregion
}