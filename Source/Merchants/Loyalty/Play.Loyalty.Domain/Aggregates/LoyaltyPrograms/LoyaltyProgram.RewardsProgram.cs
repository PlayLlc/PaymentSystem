using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;

using System.Security.Policy;

using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Domain.Aggregates;

public partial class LoyaltyProgram
{
    #region Instance Members

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task UpdateRewardsProgram(IRetrieveUsers userService, UpdateRewardsProgram command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardsProgram.GetRewardAmount().GetNumericCurrencyCode(), command.RewardAmount));

        _RewardsProgram.Update(command);

        Publish(new RewardsProgramHasBeenUpdated(this));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task ActivateRewardsProgram(IRetrieveUsers userService, ActivateRewardsProgram command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyProgram>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyProgram>(_MerchantId, user));
        _RewardsProgram.Activate(command.IsActive);

        Publish(new RewardsProgramActiveStatusHasBeenUpdated(this, command.UserId, command.IsActive));
    }

    internal uint CalculateEarnedPoints(Money transactionAmount) => _RewardsProgram.CalculateEarnedPoints(transactionAmount);

    internal uint CalculateRewards(uint points, out Money? reward) => _RewardsProgram.CalculateRewards(points, out reward);

    #endregion
}