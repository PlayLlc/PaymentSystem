using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Commands;
using Play.Domain.ValueObjects;
using Play.Loyalty.Domain.Aggregates._Shared.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;

using System.Security.Policy;

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
        Enforce(new CurrencyMustBeValid(_RewardsProgram.GetRewardAmount().GetNumericCurrencyCode(), command.Reward));

        _RewardsProgram.Update(command);

        Publish(new RewardsProgramHasBeenUpdated(this));
    }

    #endregion
}