using NServiceBus;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Identity.Contracts;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Application.Handlers;

public class StoreHandler : IHandleMessages<StoreHasBeenDeletedEvent>, IHandleMessages<StoreHasBeenCreatedEvent>
{
    #region Instance Values

    private readonly IInventoryRepository _InventoryRepository;
    private readonly IItemRepository _ItemRepository;
    private readonly IRetrieveUsers _UserRetriever;

    #endregion

    #region Constructor

    public StoreHandler(IInventoryRepository inventoryRepository, IItemRepository itemRepository, IRetrieveUsers userRetriever)
    {
        _InventoryRepository = inventoryRepository;
        _ItemRepository = itemRepository;
        _UserRetriever = userRetriever;
    }

    #endregion

    #region Instance Members

    /// <exception cref=" NotFoundException"></exception>
    /// <exception cref=" BusinessRuleValidationException"></exception>
    /// <exception cref=" ValueObjectException"></exception>
    public async Task Handle(StoreHasBeenDeletedEvent message, IMessageHandlerContext context)
    {
        IEnumerable<Item> inventoryItems = await _ItemRepository.GetItemsAsync(new(message.MerchantId), new(message.StoreId))
            .ConfigureAwait(false);

        foreach (Item item in inventoryItems)
            await item.RemoveStore(_UserRetriever, new() {StoreIds = new List<string> {message.StoreId}}).ConfigureAwait(false);

        Domain.Aggregates.Inventory? inventory = await _InventoryRepository.GetByStoreIdAsync(new(message.StoreId)).ConfigureAwait(false);

        if (inventory is null)
            return;

        // BUG: remember cascade delete you haven't cascaded anything yet explicitly
        await inventory.DeleteInventory().ConfigureAwait(false);
    }

    public async Task Handle(StoreHasBeenCreatedEvent message, IMessageHandlerContext context)
    {
        IEnumerable<Item> items = await _ItemRepository.GetItemsWithAllLocationsSet(new(message.MerchantId)).ConfigureAwait(false);

        await Domain.Aggregates.Inventory.CreateInventory(message.MerchantId, message.StoreId,
                items.Select(a => a.Id))
            .ConfigureAwait(false);
    }

    #endregion
}