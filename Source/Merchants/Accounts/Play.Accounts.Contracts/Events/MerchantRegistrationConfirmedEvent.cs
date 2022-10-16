using Play.Messaging.NServiceBus;

namespace Play.Accounts.Contracts.Events;

public class MerchantRegistrationConfirmedEvent : NetworkEvent
{
    #region Instance Values

    public string MerchantRegistrationId { get; set; } = string.Empty;

    #endregion
}