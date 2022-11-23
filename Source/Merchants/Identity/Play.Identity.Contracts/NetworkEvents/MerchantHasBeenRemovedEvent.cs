using Play.Messaging.NServiceBus;

namespace Play.Identity.Contracts;

public class MerchantHasBeenRemovedEvent : NetworkEvent
{
    #region Instance Values

    public string MerchantId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    #endregion
}