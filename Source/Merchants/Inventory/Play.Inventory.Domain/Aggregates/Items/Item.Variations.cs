using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

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
        Variation variation = new Variation(new SimpleStringId(id), name, price, sku);

        if (!_Variations.Add(variation))
            return;

        Publish(new VariationCreated(this, variation, user.GetId()));
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

        Publish(new VariationRemoved(this, command.VariationId, user.GetId()));
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
        variation.UpdateName(command.Name);
        Publish(new VariationNameUpdated(this, variation, user.GetId(), command.Name));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task UpdatePrice(IRetrieveUsers userService, UpdateItemPrice command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemPriceMustBePositive(command.Price));
        Enforce(new ItemVariationMustExist(_Variations, command.VariationId));

        Variation variation = _Variations.First(a => a.Id == command.VariationId);
        variation.UpdatePrice(command.Price);
        Publish(new PriceUpdated(this, variation, user.GetId(), command.Price));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task UpdateSku(IRetrieveUsers userService, UpdateItemSku command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemVariationMustExist(_Variations, command.VariationId));

        Variation variation = _Variations.First(a => a.GetId() == command.VariationId);
        Sku sku = new Sku(command.Sku);
        variation.UpdateSku(sku);
        Publish(new SkuUpdated(this, variation, user.GetId(), sku));
    }

    #endregion
}