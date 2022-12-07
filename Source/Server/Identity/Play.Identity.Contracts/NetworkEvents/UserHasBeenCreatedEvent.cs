using Play.Messaging.NServiceBus;

namespace Play.Identity.Contracts;

public class UserHasBeenCreatedEvent : NetworkEvent
{
    #region Instance Values

    public string UserId { get; set; } = string.Empty;

    #endregion
}