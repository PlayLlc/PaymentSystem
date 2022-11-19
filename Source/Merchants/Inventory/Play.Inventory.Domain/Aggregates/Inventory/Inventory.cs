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

namespace Play.Inventory.Domain
{
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
        public Inventory(string id, string merchantId, string storeId, HashSet<StockItem> stockItems)
        {
            Id = new SimpleStringId(id);
            _MerchantId = new SimpleStringId(merchantId);
            _StoreId = new SimpleStringId(storeId);
            _StockItems = stockItems;
        }

        /// <exception cref="ValueObjectException"></exception>
        public Inventory(InventoryDto dto)
        {
            Id = new SimpleStringId(dto.Id);
            _StoreId = new SimpleStringId(dto.StoreId);
            _StockItems = dto.StockItems.Select(a => new StockItem(a)).ToHashSet();
        }

        #endregion

        #region Instance Members

        public override SimpleStringId GetId()
        {
            return Id;
        }

        public override InventoryDto AsDto()
        {
            return new InventoryDto()
            {
                Id = Id,
                StoreId = _StoreId,
                StockItems = _StockItems.Select(a => a.AsDto())
            };
        }

        /// <exception cref="BusinessRuleValidationException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ValueObjectException"></exception>
        public async Task CreateStockItem(IRetrieveUsers userService, CreateStockItem command)
        {
            User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
            Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
            Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
            Enforce(new StockItemMustNotAlreadyExist(_StockItems, command.VariationId));

            StockItem stockItem = new StockItem(GenerateSimpleStringId(), command.ItemId, command.VariationId, 0);
            _ = _StockItems.Add(stockItem);

            Publish(new StockItemCreated(this, stockItem, user.GetId()));
        }

        /// <exception cref="ValueObjectException"></exception>
        /// <exception cref="BusinessRuleValidationException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task RemoveStockItem(IRetrieveUsers userService, RemoveVariation command)
        {
            User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
            Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
            Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
            StockItem? stockItem = _StockItems.FirstOrDefault(a => a.VariationId == command.VariationId);

            if (stockItem is null)
                return;

            _StockItems.RemoveWhere(a => a.Id == stockItem.Id);
            Publish(new StockItemHasBeenRemoved(this, stockItem.Id));
        }

        /// <exception cref="ValueObjectException"></exception>
        /// <exception cref="BusinessRuleValidationException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task AddQuantity(IRetrieveUsers userService, UpdateStockItemQuantity command)
        {
            User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
            Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
            Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
            Enforce(new StockItemMustExist(_StockItems, command.VariationId));
            Enforce(new StockActionMustAddQuantity(command.Action));
            StockAction stockAction = new StockAction(command.Action);
            StockItem stockItem = _StockItems.First(a => a.GetId() == command.VariationId);
            stockItem.AddQuantity(command.Quantity);
            Publish(new StockItemUpdatedQuantity(this, stockItem, stockAction, command.Quantity));
        }

        /// <exception cref="ValueObjectException"></exception>
        /// <exception cref="BusinessRuleValidationException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task RemoveQuantity(IRetrieveUsers userService, IItemRepository itemRepository, UpdateStockItemQuantity command)
        {
            StockItem stockItem = _StockItems.First(a => a.GetId() == command.VariationId) ?? throw new NotFoundException(typeof(StockItem));
            Item item = await itemRepository.GetByIdAsync(new SimpleStringId(stockItem.GetItemId())).ConfigureAwait(false)
                        ?? throw new NotFoundException(typeof(Item));

            User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
            Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
            Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
            Enforce(new StockItemMustExist(_StockItems, command.VariationId));
            Enforce(new StockActionMustRemoveQuantity(command.Action));
            StockAction stockAction = new StockAction(command.Action);

            stockItem.RemoveQuantity(command.Quantity);
            _ = IsEnforced(new StockItemMustNotFallBelowThreshold(item, stockItem.Id, command.Quantity));
            _ = IsEnforced(new StockItemMustNotBeEmpty(item, stockItem.Id, command.Quantity));

            Publish(new StockItemUpdatedQuantity(this, stockItem, stockAction, command.Quantity));
        }

        #endregion
    }
}