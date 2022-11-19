using NServiceBus;

using Play.Identity.Contracts.Events;

namespace Play.Inventory.Application.Handlers;

public class StoreHandler : IHandleMessages<StoreHasBeenDeletedEvent>
{
    #region Instance Members

    public Task Handle(StoreHasBeenDeletedEvent message, IMessageHandlerContext context)
    {
        // Delete any Items that have a reference to a Store
        throw new NotImplementedException();
    }

    #endregion
}