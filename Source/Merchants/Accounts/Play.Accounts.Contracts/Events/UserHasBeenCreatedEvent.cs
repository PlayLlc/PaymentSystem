using Play.Messaging.NServiceBus;

namespace Play.Accounts.Contracts.Events;

public class UserHasBeenCreatedEvent : NetworkEvent
{
    #region Instance Values

    public string UserId { get; set; } = string.Empty;

    #endregion
}