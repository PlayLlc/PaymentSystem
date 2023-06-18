using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemIsAvailableForAllLocations>, IHandleDomainEvents<ItemLocationAdded>,
    IHandleDomainEvents<ItemLocationRemoved>
{
    #region Instance Members

    private static void SubscribeLocationsPartial(ItemHandler handler)
    {
        handler.Subscribe((IHandleDomainEvents<ItemIsAvailableForAllLocations>) handler);
        handler.Subscribe((IHandleDomainEvents<ItemLocationAdded>) handler);
        handler.Subscribe((IHandleDomainEvents<ItemLocationRemoved>) handler);
    }

    public async Task Handle(ItemLocationAdded domainEvent)
    {
        Log(domainEvent);

        foreach (string storeId in domainEvent.StoreIds)
            await CreateStockItemsForStoreInventory(storeId, domainEvent.Item.Id).ConfigureAwait(false);
    }

    private async Task CreateStockItemsForStoreInventory(string storeId, string itemId)
    {
        Domain.Aggregates.Inventory? inventory = await _InventoryRepository.GetByStoreIdAsync(new SimpleStringId(storeId)).ConfigureAwait(false)
                                                 ?? throw new NotFoundException(typeof(Domain.Aggregates.Inventory));

        await inventory.CreateStockItem(new CreateStockItem() {ItemId = itemId}); 
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