using Play.Messaging.NServiceBus;

namespace Play.Accounts.Contracts.Events;

public class MerchantHasBeenCreatedEvent : NetworkEvent
{
    #region Instance Values

    public string MerchantId { get; set; } = string.Empty;

    #endregion
}