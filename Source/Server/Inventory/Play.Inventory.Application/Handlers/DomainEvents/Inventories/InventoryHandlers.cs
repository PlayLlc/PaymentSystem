﻿using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Globalization.Time;
using Play.Inventory.Contracts;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Handlers.Inventories;

public class InventoryHandlers : DomainEventHandler, IHandleDomainEvents<LowInventoryAlert>, IHandleDomainEvents<NoInventoryAlert>,
    IHandleDomainEvents<AttemptedIncorrectStockAction>, IHandleDomainEvents<AttemptedCreatingDuplicateStockItem>, IHandleDomainEvents<StockItemCreated>,
    IHandleDomainEvents<StockItemDoesNotExist>, IHandleDomainEvents<StockItemHasBeenRemoved>, IHandleDomainEvents<StockItemUpdatedQuantity>,
    IHandleDomainEvents<InventoryItemHasBeenRemoved>
{
    #region Instance Values

    private readonly IMessageSession _MessageSession;
    private readonly IInventoryRepository _InventoryRepository;

    #endregion

    #region Constructor

    public InventoryHandlers(ILogger logger, IMessageSession messageSession, IInventoryRepository inventoryRepository) : base(logger)
    {
        _MessageSession = messageSession;
        _InventoryRepository = inventoryRepository;
        Subscribe((IHandleDomainEvents<LowInventoryAlert>) this);
        Subscribe((IHandleDomainEvents<NoInventoryAlert>) this);
        Subscribe((IHandleDomainEvents<AttemptedIncorrectStockAction>) this);
        Subscribe((IHandleDomainEvents<AttemptedCreatingDuplicateStockItem>) this);
        Subscribe((IHandleDomainEvents<StockItemCreated>) this);
        Subscribe((IHandleDomainEvents<StockItemDoesNotExist>) this);
        Subscribe((IHandleDomainEvents<StockItemHasBeenRemoved>) this);
        Subscribe((IHandleDomainEvents<StockItemUpdatedQuantity>) this);
        Subscribe((IHandleDomainEvents<InventoryItemHasBeenRemoved>) this);
    }

    #endregion

    #region Instance Members

    public async Task Handle(LowInventoryAlert domainEvent)
    {
        Log(domainEvent);

        await _MessageSession.Publish<LowInventoryAlertEvent>(a =>
            {
                a.ItemId = domainEvent.Inventory.GetId();
                a.Quantity = domainEvent.Quantity;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(StockItemCreated domainEvent)
    {
        Log(domainEvent);
        await _InventoryRepository.SaveAsync(domainEvent.Inventory).ConfigureAwait(false);
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

    public async Task Handle(AttemptedIncorrectStockAction domainEvent) =>
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The StockAction provided is not appropriate for this resource");

    public async Task Handle(AttemptedCreatingDuplicateStockItem domainEvent) =>
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely a race condition or an error in the client integration. A StockItem was attempted to be created but it already exists");

    public async Task Handle(StockItemDoesNotExist domainEvent) =>
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely a race condition or an error in the client integration. A StockItem was referenced that does not exist");

    public async Task Handle(StockItemHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _InventoryRepository.SaveAsync(domainEvent.Inventory).ConfigureAwait(false);
    }

    public async Task Handle(StockItemUpdatedQuantity domainEvent)
    {
        Log(domainEvent);
        await _InventoryRepository.SaveAsync(domainEvent.Inventory).ConfigureAwait(false);

        await _MessageSession.Publish<StockItemUpdatedEvent>(a =>
            {
                a.InventoryId = domainEvent.Inventory.Id;
                a.StockId = domainEvent.StockId;
                a.Action = domainEvent.StockAction;
                a.QuantityUpdated = domainEvent.QuantityUpdated;
                a.TotalQuantity = domainEvent.TotalQuantity;
                a.UpdatedAt = DateTimeUtc.Now;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(InventoryItemHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _InventoryRepository.RemoveByStoreIdAsync(domainEvent.Inventory.Id).ConfigureAwait(false);
    }

    #endregion
}