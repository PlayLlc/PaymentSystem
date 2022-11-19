using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemIsAvailableForAllLocations>, IHandleDomainEvents<ItemLocationAdded>,
    IHandleDomainEvents<ItemLocationRemoved>
{
    #region Instance Members

    public async Task Handle(ItemLocationAdded domainEvent)
    {
        Log(domainEvent);

        foreach (string storeId in domainEvent.StoreIds)
            await CreateStockItemsForStoreInventory(storeId, domainEvent.Item.Id, domainEvent.VariationIds).ConfigureAwait(false);
    }

    private async Task CreateStockItemsForStoreInventory(string storeId, string itemId, IEnumerable<string> variationIds)
    {
        Domain.Aggregates.Inventory? inventory = await _InventoryRepository.GetByStoreIdAsync(new SimpleStringId(storeId)).ConfigureAwait(false)
                                                 ?? throw new NotFoundException(typeof(Domain.Aggregates.Inventory));

        foreach (string variation in variationIds)
            await inventory.CreateStockItem(new CreateStockItem()
                {
                    ItemId = itemId,
                    VariationId = variation
                })
                .ConfigureAwait(false);
    }

    public async Task Handle(ItemLocationRemoved domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(ItemIsAvailableForAllLocations domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    #endregion
}