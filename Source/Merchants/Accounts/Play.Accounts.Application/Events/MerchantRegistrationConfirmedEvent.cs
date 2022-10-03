using Play.Messaging.NServiceBus;

namespace Play.Accounts.Application.Events
{
    internal class MerchantRegistrationConfirmedEvent : NetworkEvent
    {
        #region Instance Values

        public string MerchantRegistrationId { get; set; } = string.Empty;

        #endregion
    }
}