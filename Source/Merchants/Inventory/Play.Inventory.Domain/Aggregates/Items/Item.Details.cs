using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Domain.Aggregates;

/// <summary>
///     This partial takes care of the Inventory Item's basic details, like Name, SKU, Price, etc
/// </summary>
public partial class Item : Aggregate<SimpleStringId>
{
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

    #endregion
}