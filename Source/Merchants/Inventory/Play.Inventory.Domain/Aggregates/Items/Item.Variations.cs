using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain;

/// <summary>
///     This partial represents the Inventory Item's behavior related to managing Item Variations. For example, variations
///     could be created to separately track and manage variations for a T-Shirt Item. A variation could be created to
///     separately track and manage the T-Shirt's size variations: 'XSmall', 'Small', 'Medium', etc
/// </summary>
public partial class Item : Aggregate<SimpleStringId>
{
    #region Variation Add/Remove

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task CreateVariation(IRetrieveUsers userService, CreateVariation command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemVariationMustNotAlreadyExist(_Variations, command.Name));

        Name name = new Name(command.Name);
        Sku? sku = command.Sku is null ? null : new Sku(command.Sku);
        Price price = new Price(GenerateSimpleStringId(), command.Price);
        SimpleStringId id = new(GenerateSimpleStringId());
        Variation variation = new Variation(id, name, price, 0, sku);

        if (!_Variations.Add(variation))
            return;

        Publish(new ItemVariationCreated(this, variation, user.GetId()));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveVariation(IRetrieveUsers userService, RemoveVariation command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemVariationMustExist(_Variations, command.VariationId));

        if (_Variations.RemoveWhere(a => a.GetId() == command.VariationId) == 0)
            return;

        Publish(new ItemVariationRemoved(this, command.VariationId, user.GetId()));
    }

    #endregion

    #region Variation Item Details

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateVariationName(IRetrieveUsers userService, UpdateItemVariationName command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemVariationMustExist(_Variations, command.VariationId));

        Variation variation = _Variations.First(a => a.GetId() == command.VariationId);
        variation.Name = new Name(command.Name);
        Publish(new VariationNameUpdated(this, variation, user.GetId(), _Name));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task UpdateVariationPrice(IRetrieveUsers userService, UpdateItemVariationPrice command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemVariationMustExist(_Variations, command.VariationId));
        Enforce(new ItemPriceMustBePositive(command.Price));

        Variation variation = _Variations.First(a => a.GetId() == command.VariationId);
        variation.Price.Amount = command.Price.GetAmount();
        Publish(new VariationPriceUpdated(this, variation, user.GetId(), command.Price));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateVariationSku(IRetrieveUsers userService, UpdateItemVariationSku command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemVariationMustExist(_Variations, command.VariationId));

        Variation variation = _Variations.First(a => a.GetId() == command.VariationId);
        variation.Sku = new Sku(command.Sku);
        Publish(new VariationSkuUpdated(this, variation, user.GetId(), _Sku));
    }

    #endregion

    #region Variation Quantity

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task AddQuantityToVariation(IRetrieveUsers userService, UpdateQuantityToInventoryForVariation command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemVariationMustExist(_Variations, command.VariationId));
        Enforce(new StockActionMustAddQuantity(command.Action));
        Variation variation = _Variations.First(a => a.GetId() == command.VariationId);

        StockAction stockAction = new StockAction(command.Action);
        variation.Quantity += command.Quantity;
        Publish(new VariationStockUpdated(this, variation, user.GetId(), stockAction, command.Quantity));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveQuantityFromVariation(IRetrieveUsers userService, UpdateQuantityToInventoryForVariation command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemVariationMustExist(_Variations, command.VariationId));
        Enforce(new StockActionMustRemoveQuantity(command.Action));
        StockAction stockAction = new StockAction(command.Action);
        Variation variation = _Variations.First(a => a.GetId() == command.VariationId);

        variation.Quantity -= command.Quantity;
        _ = IsEnforced(new ItemStockMustNotFallBeLow(_Alerts, _Quantity));
        _ = IsEnforced(new ItemStockMustNotBeEmpty(_Alerts, _Quantity));

        Publish(new VariationStockUpdated(this, variation, user.GetId(), stockAction, command.Quantity));
    }

    #endregion
}