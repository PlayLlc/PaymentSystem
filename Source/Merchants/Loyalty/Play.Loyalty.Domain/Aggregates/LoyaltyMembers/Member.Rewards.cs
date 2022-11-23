using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.Aggregates.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

using System.Transactions;

namespace Play.Loyalty.Domain.Aggregates;

public partial class LoyaltyMember : Aggregate<SimpleStringId>
{
    #region Instance Members

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task AddRewardPoints(IRetrieveUsers userRetriever, ILoyaltyProgramRepository loyaltyProgramRepository, UpdateRewardsPoints command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyMember>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyMember>(_MerchantId, user));

        LoyaltyProgram loyaltyProgram = await loyaltyProgramRepository.GetByIdAsync(new SimpleStringId(command.MerchantId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        if (!loyaltyProgram.IsRewardProgramActive())
            return;

        uint points = loyaltyProgram.CalculateEarnedPoints(command.TransactionAmount) + _Rewards.GetPoints();
        _Rewards.UpdatePoints(loyaltyProgram.CalculateRewards(points, out Money? reward));
        Publish(new LoyaltyMemberEarnedPoints(this, _MerchantId, user.Id, command.TransactionId, points));

        if (reward is null)
            return;

        _Rewards.AddToBalance(reward);

        Publish(new LoyaltyMemberEarnedRewards(this, _MerchantId, user.Id, command.TransactionId, reward));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task RemoveReward(IRetrieveUsers userRetriever, ILoyaltyProgramRepository loyaltyProgramRepository, UpdateRewardsPoints command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyMember>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyMember>(_MerchantId, user));
        LoyaltyProgram loyaltyProgram = await loyaltyProgramRepository.GetByIdAsync(new SimpleStringId(command.MerchantId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        if (!loyaltyProgram.IsRewardProgramActive())
            return;

        uint lostPoints = loyaltyProgram.CalculateEarnedPoints(command.TransactionAmount) + _Rewards.GetPoints();

        if (lostPoints >= _Rewards.GetPoints())
        {
            _Rewards.SubtractPoints(lostPoints);
            Publish(new LoyaltyMemberLostPoints(this, _MerchantId, user.Id, command.TransactionId, lostPoints));
        }

        ConvertRewardsToPoints(loyaltyProgram, user.Id, command.TransactionId, lostPoints);
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task ClaimRewards(IRetrieveUsers userRetriever, ILoyaltyProgramRepository loyaltyProgramRepository, ClaimRewards command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyMember>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyMember>(_MerchantId, user));
        LoyaltyProgram loyaltyProgram = await loyaltyProgramRepository.GetByMerchantIdAsync(_MerchantId).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        Enforce(new RewardsProgramMustBeActiveToClaimReward(loyaltyProgram));
        Enforce(new RewardsBalanceMustBeGreaterThanOrEqualToRewardRedemption(command.RewardAmount, _Rewards.GetRewardBalance()));

        _Rewards.Claim(command.RewardAmount);
        Publish(new LoyaltyMemberClaimedRewards(this, _MerchantId, user.Id, command.TransactionId, command.RewardAmount));
    }

    private void ZeroOutRewards(string userId, uint transactionId, uint points, Money rewardsBalance)
    {
        _Rewards.UpdatePoints(0);
        Publish(new LoyaltyMemberLostPoints(this, _MerchantId, userId, transactionId, points));

        if (rewardsBalance.GetAmount() == 0)
            return;

        _Rewards.SubtractFromBalancce(rewardsBalance);
        Publish(new LoyaltyMemberLostRewards(this, _MerchantId, userId, transactionId, rewardsBalance));
    }

    private void ConvertRewardsToPoints(LoyaltyProgram loyaltyProgram, string userId, uint transactionId, uint lostPoints)
    {
        uint points = _Rewards.GetPoints();
        Money rewardsBalance = _Rewards.GetRewardBalance();
        uint rewardsAsPoints = loyaltyProgram.CalculateEarnedPoints(_Rewards.GetRewardBalance());

        // After we've converted any rewards the customer has earned to points, if there's still not enough
        // points to compensate for the lost points, we'll just zero out their points and their Rewards Balance
        if (lostPoints > (rewardsAsPoints + points))
        {
            ZeroOutRewards(userId, points, transactionId, rewardsBalance);

            return;
        }

        // Calculate the Loyalty Member's new point total by subtracting the points they lost from a return 
        _Rewards.UpdatePoints(loyaltyProgram.CalculateRewards((rewardsAsPoints + points) - lostPoints, out Money? rewards));
        Publish(new LoyaltyMemberLostPoints(this, _MerchantId, userId, transactionId, points));

        // After calculating the Loyalty Member's new point total, if there is still a Rewards Balance, update
        // their new balance
        if (rewards is null)
            return;

        _Rewards.UpdateBalance(rewards);
        Publish(new LoyaltyMemberLostRewards(this, _MerchantId, userId, transactionId, rewardsBalance));
    }

    #endregion
}