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
    public async Task CreateDiscountedItem(IRetrieveUsers userService, CreateDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardsProgram.GetRewardAmount().GetNumericCurrencyCode(), command.DiscountedPrice));
        Enforce(new DiscountMustNotExist(_Discounts, command.ItemId, command.VariationId));

        _ = _Discounts.Add(new Discount(new string(GenerateSimpleStringId()), command.ItemId, command.VariationId, command.DiscountedPrice));

        Publish(new DiscountHasBeenCreated(this, command.ItemId, command.VariationId, command.DiscountedPrice));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task UpdateDiscountedPrice(IRetrieveUsers userService, UpdateDiscountedPrice command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardsProgram.GetRewardAmount().GetNumericCurrencyCode(), command.DiscountedPrice));
        Enforce(new DiscountMustExist(_Discounts, command.DiscountId));

        _Discounts.First(a => a.Id == command.DiscountId).UpdateDiscountPrice(command.DiscountedPrice);

        Publish(new DiscountHasBeenUpdated(this, command.DiscountId, command.DiscountedPrice));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task RemoveDiscountedItemPrice(IRetrieveUsers userService, RemoveDiscountedItemPrice command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new DiscountMustExist(_Discounts, command.DiscountId));

        _Discounts.RemoveWhere(a => a.Id == command.DiscountId);

        Publish(new DiscountHasBeenRemoved(this, command.DiscountId));
    }

    #endregion
}