using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Domain.Aggregates;

/// <summary>
///     This partial takes care of managing which stores receives and manages a stock of this Inventory Item
/// </summary>
public partial class Item : Aggregate<SimpleStringId>
{
    #region Locations

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task SetAllLocations(IRetrieveUsers userService, SetAllLocationsForItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));

        if (_Locations.IsAllLocationsSet())
            return;

        _Locations.SetAllLocations();
        Publish(new ItemIsAvailableForAllLocations(this, command.UserId));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task AddStore(IRetrieveUsers userService, UpdateItemLocations command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));

        if (_Locations.AddLocations(command.StoreIds.Select(a => new SimpleStringId(a))) == 0)
            return;

        Publish(new ItemLocationAdded(this, user.GetId(), command.StoreIds));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task RemoveStore(IRetrieveUsers userService, UpdateItemLocations command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));

        if (_Locations.AddLocations(command.StoreIds.Select(a => new SimpleStringId(a))) == 0)
            return;

        Publish(new ItemLocationRemoved(this, user.GetId(), command.StoreIds));
    }

    #endregion
}