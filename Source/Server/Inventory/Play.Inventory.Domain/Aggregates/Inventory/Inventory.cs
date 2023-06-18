using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

public class Inventory : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly SimpleStringId _StoreId;

    private readonly HashSet<StockItem> _StockItems;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Inventory(string id, string merchantId, string storeId, IEnumerable<StockItem> stockItems)
    {
        Id = new(id);
        _MerchantId = new(merchantId);
        _StoreId = new(storeId);
        _StockItems = stockItems.ToHashSet();
    }

    /// <exception cref="ValueObjectException"></exception>
    public Inventory(InventoryDto dto)
    {
        Id = new(dto.Id);
        _StoreId = new(dto.StoreId);
        _StockItems = dto.StockItems.Select(a => new StockItem(a)).ToHashSet();
    }

    #endregion

    #region Instance Members

    internal string GetStoreId() => _StoreId;

    public override SimpleStringId GetId() => Id;

    public override InventoryDto AsDto()
    {
        return new()
        {
            Id = Id,
            StoreId = _StoreId,
            StockItems = _StockItems.Select(a => a.AsDto())
        };
    }

    public static Task CreateInventory(string merchantId, string storeId, IEnumerable<SimpleStringId> itemIds)
    {
        List<StockItem> stockItems = new();
        foreach (var itemId in itemIds)
            stockItems.Add(new StockItem(GenerateSimpleStringId(), itemId, 0));

        Inventory inventory = new(GenerateSimpleStringId(), merchantId, storeId, stockItems);

        inventory.Publish(new InventoryCreated(inventory));

        return Task.CompletedTask;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task CreateStockItem(CreateStockItem command)
    {
        Enforce(new StockItemMustNotAlreadyExist(_StockItems, command.ItemId));

        StockItem stockItem = new(GenerateSimpleStringId(), command.ItemId, 0);
        _ = _StockItems.Add(stockItem);

        await Publish(new StockItemCreated(this, stockItem)).ConfigureAwait(false); 
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveStockItem(IRetrieveUsers userService, RemoveStockItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Inventory>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Inventory>(_MerchantId, user));
        StockItem? stockItem = _StockItems.FirstOrDefault(a => a.Id == command.ItemId);

        if (stockItem is null)
            return;

        _StockItems.RemoveWhere(a => a.Id == stockItem.Id);
        Publish(new StockItemHasBeenRemoved(this, stockItem.Id));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public Task DeleteInventory()
    {
        Publish(new InventoryItemHasBeenRemoved(this));

        return Task.CompletedTask;
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task AddQuantity(IRetrieveUsers userService, UpdateStockItemQuantity command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Inventory>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Inventory>(_MerchantId, user));
        Enforce(new StockItemMustExist(_StockItems, command.StockItemId));
        Enforce(new StockActionMustAddQuantity(command.Action));
        StockAction stockAction = new(command.Action);
        StockItem stockItem = _StockItems.First(a => a.GetId() == command.StockItemId);
        stockItem.AddQuantity(command.Quantity);
        Publish(new StockItemUpdatedQuantity(this, stockItem.Id, stockAction, command.Quantity, stockItem.GetQuantity()));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveQuantity(IRetrieveUsers userService, IItemRepository itemRepository, UpdateStockItemQuantity command)
    {
        StockItem stockItem = _StockItems.First(a => a.GetId() == command.StockItemId) ?? throw new NotFoundException(typeof(StockItem));
        Item item = await itemRepository.GetByIdAsync(new(stockItem.GetItemId())).ConfigureAwait(false)
                    ?? throw new NotFoundException(typeof(Item));

        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Inventory>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Inventory>(_MerchantId, user));
        Enforce(new StockItemMustExist(_StockItems, command.StockItemId));
        Enforce(new StockActionMustRemoveQuantity(command.Action));
        StockAction stockAction = new(command.Action);

        stockItem.RemoveQuantity(command.Quantity);
        _ = IsEnforced(new StockItemMustNotFallBelowThreshold(item, stockItem.Id, command.Quantity));
        _ = IsEnforced(new StockItemMustNotBeEmpty(item, stockItem.Id, command.Quantity));

        Publish(new StockItemUpdatedQuantity(this, stockItem.Id, stockAction, command.Quantity, stockItem.GetQuantity()));
    } //

    #endregion
}