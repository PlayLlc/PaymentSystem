using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NServiceBus;

using Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration;
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