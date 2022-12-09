using NServiceBus;

using Play.Domain.Events;
using Play.Inventory.Contracts;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemAlertsHaveBeenActivated>, IHandleDomainEvents<ItemAlertsHaveBeenDeactivated>,
    IHandleDomainEvents<LowInventoryItemThresholdUpdated>, IHandleDomainEvents<NoInventoryAlert>, IHandleDomainEvents<AttemptedIncorrectStockAction>

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

    public async Task Handle(NoInventoryAlert domainEvent)
    {
        Log(domainEvent);
        await _MessageSession.Publish<NoInventoryAlertEvent>(a =>
            {
                a.ItemId = domainEvent.Item.GetId();
            })
            .ConfigureAwait(false);
    }

    // When we handle this domain event, it means there's a problem with the client implementation
    public Task Handle(AttemptedIncorrectStockAction domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    #endregion
}