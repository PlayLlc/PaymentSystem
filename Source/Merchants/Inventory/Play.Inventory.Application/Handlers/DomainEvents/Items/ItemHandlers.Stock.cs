using Play.Domain.Events;

using NServiceBus;

using Play.Globalization.Time;
using Play.Inventory.Contracts.Events;
using Play.Inventory.Domain;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemAlertsHaveBeenActivated>, IHandleDomainEvents<ItemAlertsHaveBeenDeactivated>,
    IHandleDomainEvents<ItemStockUpdated>, IHandleDomainEvents<LowInventoryAlert>, IHandleDomainEvents<LowInventoryItemThresholdUpdated>,
    IHandleDomainEvents<NoInventoryAlert>, IHandleDomainEvents<StockActionWasIncorrect>

{
    #region Instance Members

    public async Task Handle(ItemAlertsHaveBeenActivated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(ItemAlertsHaveBeenDeactivated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(LowInventoryItemThresholdUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(LowInventoryAlert domainEvent)
    {
        Log(domainEvent);

        await _MessageHandlerContext.Publish<LowInventoryAlertEvent>((a) =>
            {
                a.ItemId = domainEvent.Item.GetId();
                a.QuantitySubtotal = domainEvent.Item.GetQuantityInStock();
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(NoInventoryAlert domainEvent)
    {
        Log(domainEvent);
        await _MessageHandlerContext.Publish<NoInventoryAlertEvent>((a) =>
            {
                a.ItemId = domainEvent.Item.GetId();
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(ItemStockUpdated domainEvent)
    {
        Log(domainEvent);

        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<ItemStockUpdatedEvent>((a) =>
            {
                a.ItemId = domainEvent.Item.GetId();
                a.Action = domainEvent.Action;
                a.Quantity = domainEvent.Quantity;
                a.QuantitySubtotal = domainEvent.Item.GetQuantityInStock();
                a.UpdatedAt = DateTimeUtc.Now;
            })
            .ConfigureAwait(false);
    }

    // When we handle this domain event, it means there's a problem with the client implementation
    public Task Handle(StockActionWasIncorrect domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    #endregion
}