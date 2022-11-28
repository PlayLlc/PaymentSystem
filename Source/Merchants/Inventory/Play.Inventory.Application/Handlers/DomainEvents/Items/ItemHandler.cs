using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Domain.Repositories;

using NServiceBus;

using Play.Domain.Common.ValueObjects;
using Play.Inventory.Contracts;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemCreated>, IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Item>>,
    IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Item>>, IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Item>>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IInventoryRepository _InventoryRepository;
    private readonly IRepository<Item, SimpleStringId> _ItemRepository;

    #endregion

    #region Constructor

    public ItemHandler(
        ILogger<ItemHandler> logger, IMessageHandlerContext messageHandlerContext, IRepository<Item, SimpleStringId> itemRepository,
        IInventoryRepository inventoryRepository) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _ItemRepository = itemRepository;
        _InventoryRepository = inventoryRepository;
    }

    #endregion

    #region Instance Members

    public async Task Handle(ItemCreated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);

        // Network Event: Send to reporting layer and any other subdomains that are interested. We will keep a commit log of inventory updates
        await _MessageHandlerContext.Publish<InventoryItemCreatedEvent>((a) =>
            {
                a.Item = domainEvent.Item.AsDto();
                a.UserId = domainEvent.UserId;
            })
            .ConfigureAwait(false);
    }

    public Task Handle(AggregateUpdateWasAttemptedByUnknownUser<Item> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            $"\n\n\n\nWARNING: There is likely an error in the client integration. The User is not associated with the specified Merchant");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedMerchantAttemptedToCreateAggregate<Item> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The Merchant is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedUserAttemptedToUpdateAggregate<Item> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            $"\n\n\n\nWARNING: There is likely an error in the client integration. The User is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    #endregion
}