using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NServiceBus;

using Play.Accounts.Contracts.Events;

namespace Play.Terminals.Application.Handlers.Network
{
    public class MerchantRegistrationConfirmedHandler : IHandleMessages<MerchantRegistrationConfirmedEvent>
    {
        #region Instance Members

        public async Task Handle(MerchantRegistrationConfirmedEvent message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}