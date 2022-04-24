namespace Play.Messaging;

internal record EventHeader
{
    #region Instance Values

    private readonly MessagingConfiguration? _MessagingConfiguration;

    #endregion

    #region Constructor

    public EventHeader()
    { }

    public EventHeader(MessagingConfiguration? messagingConfiguration)
    {
        _MessagingConfiguration = messagingConfiguration;
    }

    #endregion

    #region Instance Members

    public bool TryGetMessagingConfiguration(out MessagingConfiguration? result)
    {
        if (_MessagingConfiguration is null)
        {
            result = default;

            return false;
        }

        result = _MessagingConfiguration!;

        return true;
    }

    #endregion
}