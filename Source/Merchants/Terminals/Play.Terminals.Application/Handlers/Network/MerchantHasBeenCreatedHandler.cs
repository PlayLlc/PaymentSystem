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