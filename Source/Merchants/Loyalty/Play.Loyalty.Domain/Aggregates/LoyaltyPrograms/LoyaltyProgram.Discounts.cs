using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Aggregates;

public partial class LoyaltyProgram
{
    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task CreateDiscountedItem(IRetrieveUsers userService, IRetrieveInventoryItems inventoryItemRetriever, CreateDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardsProgram.GetRewardAmount().GetNumericCurrencyCode(), command.DiscountedPrice));
        Enforce(new DiscountMustNotExist(_DiscountsProgram, command.ItemId, command.VariationId));
        Enforce(new DiscountPriceMustBeLowerThanItemPrice(
            await inventoryItemRetriever.GetByIdAsync(command.ItemId, command.VariationId).ConfigureAwait(false)
            ?? throw new NotFoundException(typeof(InventoryItem)), command.DiscountedPrice.AsMoney()));
        Discount discount = new Discount(GenerateSimpleStringId(), command.ItemId, command.VariationId, command.DiscountedPrice);
        _ = _DiscountsProgram.Add(discount);

        Publish(new DiscountHasBeenCreated(this, discount, command.ItemId, command.VariationId, command.DiscountedPrice));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateDiscountedItem(IRetrieveUsers userService, UpdateDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardsProgram.GetRewardAmount().GetNumericCurrencyCode(), command.Price));
        Enforce(new DiscountMustExist(_DiscountsProgram, command.DiscountId));

        _DiscountsProgram.UpdateDiscountPrice(command.DiscountId, command.Price);

        Publish(new DiscountHasBeenUpdated(this, command.DiscountId, command.Price));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task RemoveDiscountedItem(IRetrieveUsers userService, RemoveDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new DiscountMustExist(_DiscountsProgram, command.DiscountId));
        _DiscountsProgram.Remove(command.DiscountId);

        Publish(new DiscountHasBeenRemoved(this, command.DiscountId));
    }

    #endregion
}