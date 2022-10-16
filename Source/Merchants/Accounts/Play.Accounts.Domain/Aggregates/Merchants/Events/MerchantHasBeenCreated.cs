using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.Merchants.Events
{
    public record MerchantHasBeenCreated : DomainEvent
    {
        #region Instance Values

        public readonly string MerchantId;

        #endregion

        #region Constructor

        public MerchantHasBeenCreated(string merchantId) : base($"The merchant with {nameof(merchantId)}: [{merchantId}] has been created")
        {
            MerchantId = merchantId;
        }

        #endregion
    }
}