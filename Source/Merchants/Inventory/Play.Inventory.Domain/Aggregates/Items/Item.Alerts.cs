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
    #region Alerts

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task ActivateAlerts(IRetrieveUsers userService, UpdateItemAlerts command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        _Alerts.ActivateAlerts();
        Publish(new ItemAlertsHaveBeenActivated(this, user.GetId()));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task DeactivateAlerts(IRetrieveUsers userService, UpdateItemAlerts command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

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