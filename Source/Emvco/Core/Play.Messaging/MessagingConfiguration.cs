namespace Play.Messaging;

public record MessagingConfiguration
{
    #region Instance Values

    private readonly TimeoutConfiguration? _TimeoutConfiguration;

    #endregion

    #region Constructor

    public MessagingConfiguration(TimeoutConfiguration? timeoutConfiguration)
    {
        _TimeoutConfiguration = timeoutConfiguration;
    }

    #endregion

    #region Instance Members

    public TimeoutConfiguration? GetTimeoutConfiguration() => _TimeoutConfiguration;

    #endregion
}