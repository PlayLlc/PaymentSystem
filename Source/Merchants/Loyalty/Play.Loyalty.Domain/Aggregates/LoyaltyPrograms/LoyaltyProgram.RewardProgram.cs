using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Aggregatesd;
using Play.Domain.ValueObjects;
using Play.Loyalty.Domain.Aggregates._Shared.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Aggregates;

public partial class LoyaltyProgram
{
    #region Instance Members

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task SetRewardType(IRetrieveUsers userService, SetRewardType command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));

        _RewardProgram.SetRewardType(command.RewardType);

        Publish(new RewardTypeHasBeenSet(this, command.RewardType));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task ActivateRewardProgram(IRetrieveUsers userService, ActivateRewardProgram command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));

        _RewardProgram.Activate(command.Activate);

        Publish(new RewardProgramActivationHasBeenUpdated(this, command.Activate));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateAmountOff(IRetrieveUsers userService, UpdateAmountOff command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardProgram.GetAmountOff().GetNumericCurrencyCode(), command.AmountOff));

        _RewardProgram.UpdateAmountOff(command.AmountOff);

        Publish(new AmountOffHasBeenUpdated(this, command.AmountOff));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task UpdatePercentageOff(IRetrieveUsers userService, IRetrieveMerchants merchantService, UpdatePercentageOff command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantService.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(merchant.Id, user));
        Enforce(new PercentageMustBeValid(command.Percentage));

        _RewardProgram.UpdatePercentageOff(command.Percentage);

        Publish(new PercentageOffHasBeenUpdated(this, command.Percentage, merchant.Id));
    }

    #endregion
}