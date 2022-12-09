using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.Aggregates._External;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Aggregates;

public partial class Programs
{
    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task CreateDiscountedItem(IRetrieveUsers userService, IRetrieveInventoryItems inventoryItemRetriever, CreateDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Programs>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Programs>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardProgram.GetRewardAmount().GetNumericCurrencyCode(), command.DiscountedPrice));
        Enforce(new DiscountMustNotExist(_DiscountProgram, command.ItemId, command.VariationId));
        Enforce(new DiscountPriceMustBeLowerThanItemPrice(
            await inventoryItemRetriever.GetByIdAsync(command.ItemId, command.VariationId).ConfigureAwait(false)
            ?? throw new NotFoundException(typeof(InventoryItem)), command.DiscountedPrice.AsMoney()));
        Discount discount = new(GenerateSimpleStringId(), command.ItemId, command.VariationId, command.DiscountedPrice);
        _ = _DiscountProgram.Add(discount);

        Publish(new DiscountHasBeenCreated(this, discount, command.UserId, command.ItemId, command.VariationId, command.DiscountedPrice));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateDiscountedItem(IRetrieveUsers userService, UpdateDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Programs>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Programs>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardProgram.GetRewardAmount().GetNumericCurrencyCode(), command.Price));
        Enforce(new DiscountMustExist(_DiscountProgram, command.DiscountId));

        _DiscountProgram.UpdateDiscountPrice(command.DiscountId, command.Price);

        Publish(new DiscountPriceHasBeenUpdated(this, command.DiscountId, command.UserId, command.Price));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task RemoveDiscountedItem(IRetrieveUsers userService, RemoveDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Programs>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Programs>(_MerchantId, user));
        Enforce(new DiscountMustExist(_DiscountProgram, command.DiscountId));
        _DiscountProgram.Remove(command.DiscountId);

        Publish(new DiscountHasBeenRemoved(this, command.DiscountId, command.UserId));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task ActivateDiscountProgram(IRetrieveUsers userService, ActivateProgram command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<Programs>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Programs>(_MerchantId, user));
        _DiscountProgram.Activate(command.IsActive);

        Publish(new RewardsProgramActiveStatusHasBeenUpdated(this, command.UserId, command.IsActive));
    }

    #endregion
}