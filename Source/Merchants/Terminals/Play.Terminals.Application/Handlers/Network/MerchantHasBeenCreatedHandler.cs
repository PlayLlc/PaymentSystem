using NServiceBus;

namespace Play.Terminals.Application.Handlers.Network;

public class MerchantHasBeenCreatedHandler : IHandleMessages<MerchantHasBeenCreatedEvent>
{
    #region Instance Members

    public Task Handle(MerchantHasBeenCreatedEvent message, IMessageHandlerContext context) =>
        throw

            // Begin to create Terminal configuration for MerchantRegistrationId
            new NotImplementedException();

    #endregion
}