﻿using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Aggregates;

public partial class Programs
{
    #region Instance Members

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task UpdateRewardsProgram(IRetrieveUsers userService, UpdateRewardsProgram command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<Programs>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Programs>(_MerchantId, user));
        Enforce(new CurrencyMustBeValid(_RewardProgram.GetRewardAmount().GetNumericCurrencyCode(), command.RewardAmount));

        _RewardProgram.Update(command);

        Publish(new RewardsProgramHasBeenUpdated(this, user.Id));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task ActivateRewardProgram(IRetrieveUsers userService, ActivateProgram command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<Programs>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Programs>(_MerchantId, user));
        _RewardProgram.Activate(command.IsActive);

        Publish(new RewardsProgramActiveStatusHasBeenUpdated(this, command.UserId, command.IsActive));
    }

    internal uint CalculateEarnedPoints(Money transactionAmount) => _RewardProgram.CalculateEarnedPoints(transactionAmount);

    internal uint CalculateRewards(uint points, out Money? reward) => _RewardProgram.CalculateRewards(points, out reward);

    #endregion
}