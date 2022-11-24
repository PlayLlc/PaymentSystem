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
        Discount discount = new Discount(new string(GenerateSimpleStringId()), command.ItemId, command.VariationId, command.DiscountedPrice);
        _ = _Discounts.Add(discount);

        Publish(new DiscountHasBeenCreated(this, discount, command.ItemId, command.VariationId, command.DiscountedPrice));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task UpdateDiscountedItem(IRetrieveUsers userService, UpdateDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardsProgram.GetRewardAmount().GetNumericCurrencyCode(), command.DiscountedPrice));
        Enforce(new DiscountMustExist(_Discounts, command.DiscountId));

        Discount discount = _Discounts.First(a => a.Id == command.DiscountId);

        discount.UpdateDiscountPrice(command.DiscountedPrice);

        Publish(new DiscountHasBeenUpdated(this, discount, command.DiscountedPrice));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task RemoveDiscountedItem(IRetrieveUsers userService, RemoveDiscountedItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new DiscountMustExist(_Discounts, command.DiscountId));

        Discount discount = _Discounts.First(a => a.Id == command.DiscountId);

        _Discounts.RemoveWhere(a => a.Id == command.DiscountId);

        Publish(new DiscountHasBeenRemoved(this, discount));
    }

    #endregion
}