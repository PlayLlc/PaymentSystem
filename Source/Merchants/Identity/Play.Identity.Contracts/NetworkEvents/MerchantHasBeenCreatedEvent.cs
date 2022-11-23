using Play.Messaging.NServiceBus;

namespace Play.Identity.Contracts;

public record MerchantHasBeenCreatedEvent : NetworkEvent
{
    #region Instance Values

    public string MerchantId { get; set; } = string.Empty;

    #endregion
}