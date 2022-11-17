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
///     This partial represents the Inventory Item's behavior related to managing stock for this item
/// </summary>
public partial class Item : Aggregate<SimpleStringId>
{
    #region Quantity

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task AddQuantity(IRetrieveUsers userService, AddQuantityToInventory command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new StockActionMustAddQuantity(command.Action));
        StockAction stockAction = new StockAction(command.Action);
        _Quantity += command.Quantity;
        Publish(new ItemStockUpdated(this, user.GetId(), stockAction, command.Quantity));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveQuantity(IRetrieveUsers userService, RemoveQuantityFromInventory command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new StockActionMustRemoveQuantity(command.Action));

        StockAction stockAction = new StockAction(command.Action);
        _Quantity -= command.Quantity;

        // Publish alerts if subscribed
        _ = IsEnforced(new ItemStockMustNotFallBeLow(_Alerts, _Quantity));
        _ = IsEnforced(new ItemStockMustNotBeEmpty(_Alerts, _Quantity));

        Publish(new ItemStockUpdated(this, user.GetId(), stockAction, command.Quantity));
    }

    #endregion

    #region Alerts

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task ActivateAlert(IRetrieveUsers userService, string userId)
    {
        User user = await userService.GetByIdAsync(userId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        _Alerts.ActivateAlerts();
        Publish(new ItemAlertsHaveBeenActivated(this, user.GetId()));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task DeactivateAlert(IRetrieveUsers userService, string userId)
    {
        User user = await userService.GetByIdAsync(userId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        _Alerts.DeactivateAlerts();
        Publish(new ItemAlertsHaveBeenDeactivated(this, user.GetId()));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateLowInventoryThreshold(IRetrieveUsers userService, UpdateLowInventoryThresholdAlert command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        _Alerts.UpdateLowInventoryThreshold(command.Quantity);
        Publish(new LowInventoryItemThresholdUpdated(this, user.GetId(), command.Quantity));
    }

    #endregion
}