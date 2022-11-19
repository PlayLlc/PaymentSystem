using Microsoft.Extensions.Logging;

using Play.Domain.Events;

using NServiceBus;

using Play.Globalization.Time;
using Play.Inventory.Contracts.Events;
using Play.Inventory.Domain;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemVariationAlreadyExists>, IHandleDomainEvents<VariationCreated>,
    IHandleDomainEvents<ItemVariationDoesNotExist>, IHandleDomainEvents<VariationRemoved>, IHandleDomainEvents<VariationNameUpdated>
{
    #region Instance Members

    public async Task Handle(VariationCreated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<InventoryItemVariationCreatedEvent>((a) =>
            {
                a.ItemId = domainEvent.Item.Id;
                a.Variation = domainEvent.Variation.AsDto();
                a.UserId = domainEvent.UserId;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(VariationRemoved domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<InventoryItemVariationRemovedEvent>((a) =>
            {
                a.ItemId = domainEvent.Item.GetId();
                a.VariationId = domainEvent.VariationId;
                a.UserId = domainEvent.UserId;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(VariationNameUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public Task Handle(ItemVariationAlreadyExists domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, $"WARNING: This likely means there is a race condition or an error in the client implementation");

        return Task.CompletedTask;
    }

    public Task Handle(ItemVariationDoesNotExist domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, $"WARNING: This likely means there is a race condition or an error in the client implementation");

        return Task.CompletedTask;
    }

    #endregion
}