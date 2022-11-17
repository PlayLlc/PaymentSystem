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
///     This partial takes care of the Inventory Item's basic details, like Name, SKU, Price, etc
/// </summary>
public partial class Item : Aggregate<SimpleStringId>
{
    #region Instance Members

    #region Create/Remove

    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task RemoveItem(IRetrieveUsers userService, RemoveItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Publish(new ItemRemoved(this, user.GetId()));
    }

    #endregion

    #endregion

    #region Item Details

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task UpdateDescription(IRetrieveUsers userService, UpdateItemDescription command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        _Description = command.Description;
        Publish(new ItemDescriptionUpdated(this, user.GetId(), _Description));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task UpdateName(IRetrieveUsers userService, UpdateItemName command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        _Name = new Name(command.Name);
        Publish(new ItemNameUpdated(this, user.GetId(), _Name));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task UpdatePrice(IRetrieveUsers userService, UpdateItemPrice command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new ItemPriceMustBePositive(command.Price));
        _Price.Amount = command.Price.GetAmount();
        Publish(new ItemPriceUpdated(this, user.GetId(), command.Price));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task UpdateSku(IRetrieveUsers userService, UpdateItemSku command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        _Sku = new Sku(command.Sku);
        Publish(new ItemSkuUpdated(this, user.GetId(), _Sku));
    }

    #endregion
}