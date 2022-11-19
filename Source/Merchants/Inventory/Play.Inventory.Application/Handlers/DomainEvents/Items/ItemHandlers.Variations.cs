using Microsoft.Extensions.Logging;

using Play.Domain.Events;

using NServiceBus;

using Play.Globalization.Time;
using Play.Inventory.Contracts.Events;
using Play.Inventory.Domain;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemVariationAlreadyExists>, IHandleDomainEvents<ItemVariationCreated>,
    IHandleDomainEvents<ItemVariationDoesNotExist>, IHandleDomainEvents<ItemVariationRemoved>, IHandleDomainEvents<VariationNameUpdated>,
    IHandleDomainEvents<VariationPriceUpdated>, IHandleDomainEvents<VariationSkuUpdated>, IHandleDomainEvents<VariationStockUpdated>
{
    #region Instance Members

    public async Task Handle(ItemVariationCreated domainEvent)
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

    public async Task Handle(ItemVariationRemoved domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<InventoryItemVariationRemovedEvent>((a) =>
            {
                a.ItemId = domainEvent.Item.GetId();
                a.VariationId = domainEvent.VariationItemId;
                a.UserId = domainEvent.UserId;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(VariationStockUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<VariationStockUpdatedEvent>((a) =>
            {
                a.ItemId = domainEvent.Item.GetId();
                a.VariationId = domainEvent.Variation.Id;
                a.Action = domainEvent.Action;
                a.Quantity = domainEvent.Quantity;
                a.QuantitySubtotal = domainEvent.Item.GetQuantityInStock();
                a.UpdatedAt = DateTimeUtc.Now;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(VariationNameUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(VariationPriceUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(VariationSkuUpdated domainEvent)
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