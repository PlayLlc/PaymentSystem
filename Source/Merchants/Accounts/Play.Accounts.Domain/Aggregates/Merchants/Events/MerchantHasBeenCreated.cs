using Play.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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