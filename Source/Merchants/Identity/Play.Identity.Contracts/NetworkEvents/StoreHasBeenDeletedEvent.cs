using Play.Messaging.NServiceBus;

namespace Play.Identity.Contracts.Events;

public class StoreHasBeenDeletedEvent : NetworkEvent
{
    #region Instance Values

    public string StoreId { get; set; } = string.Empty;
    public string MerchantId { get; set; } = string.Empty;

    #endregion
}