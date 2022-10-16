using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NServiceBus;

using Play.Accounts.Contracts.Events;

namespace Play.Terminals.Application.Handlers.Network
{
    public class MerchantHasBeenCreatedHandler : IHandleMessages<MerchantHasBeenCreatedEvent>
    {
        #region Instance Members

        public Task Handle(MerchantHasBeenCreatedEvent message, IMessageHandlerContext context)
        {
            // Begin to create Terminal configuration for MerchantRegistrationId
            throw new NotImplementedException();
        }

        #endregion
    }
}