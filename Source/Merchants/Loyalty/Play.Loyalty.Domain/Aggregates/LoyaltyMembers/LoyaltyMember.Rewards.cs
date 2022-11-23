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
using Play.Loyalty.Domain.Aggregates._Shared.Rules;
using Play.Loyalty.Domain.Aggregates.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Aggregates
{
    public partial class LoyaltyMember : Aggregate<SimpleStringId>
    {
        #region Instance Members

        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ValueObjectException"></exception>
        /// <exception cref="BusinessRuleValidationException"></exception>
        public async Task AddRewards(IRetrieveUsers userRetriever, ILoyaltyProgramRepository loyaltyProgramRepository, UpdateRewardPoints command)
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
            Publish(new LoyaltyMemberEarnedPoints(this, _MerchantId, user.Id, points));

            if (reward is null)
                return;

            _Rewards.AddToBalance(reward);

            Publish(new LoyaltyMemberEarnedRewards(this, _MerchantId, user.Id, reward));
        }

        /// <exception cref="ValueObjectException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BusinessRuleValidationException"></exception>
        public async Task ClaimRewards(IRetrieveUsers userRetriever, ILoyaltyProgramRepository loyaltyProgramRepository, ClaimReward command)
        {
            User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
            Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyMember>(user));
            Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyMember>(_MerchantId, user));
            LoyaltyProgram loyaltyProgram = await loyaltyProgramRepository.GetByMerchantIdAsync(_MerchantId).ConfigureAwait(false)
                                            ?? throw new NotFoundException(typeof(LoyaltyProgram));

            Enforce(new RewardsProgramMustBeActiveToClaimReward(loyaltyProgram));
            Enforce(new RewardMustBeGreaterThanOrEqualToClaimAmount(command.RewardAmount, _Rewards.GetRewardBalance()));

            _Rewards.Claim(command.RewardAmount);
            Publish(new LoyaltyMemberClaimedRewards(this, _MerchantId, user.Id, command.RewardAmount));
        }

        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ValueObjectException"></exception>
        /// <exception cref="BusinessRuleValidationException"></exception>
        public async Task RemoveRewards(IRetrieveUsers userRetriever, ILoyaltyProgramRepository loyaltyProgramRepository, UpdateRewardPoints command)
        {
            // Enforce rules
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
                Publish(new LoyaltyMemberLostPoints(this, _MerchantId, user.Id, lostPoints));
            }

            ConvertRewardsToPoints(loyaltyProgram, user.Id, lostPoints);
        }

        private void ZeroOutRewards(string userId, uint points, Money rewardsBalance)
        {
            _Rewards.UpdatePoints(0);
            Publish(new LoyaltyMemberLostPoints(this, _MerchantId, userId, points));

            if (rewardsBalance.GetAmount() == 0)
                return;

            _Rewards.SubtractFromBalancce(rewardsBalance);
            Publish(new LoyaltyMemberLostRewards(this, _MerchantId, userId, rewardsBalance));
        }

        private void ConvertRewardsToPoints(LoyaltyProgram loyaltyProgram, string userId, uint lostPoints)
        {
            uint points = _Rewards.GetPoints();
            Money rewardsBalance = _Rewards.GetRewardBalance();
            uint rewardsAsPoints = loyaltyProgram.CalculateEarnedPoints(_Rewards.GetRewardBalance());

            // After we've converted any rewards the customer has earned to points, if there's still not enough
            // points to compensate for the lost points, we'll just zero out their points and their Rewards Balance
            if (lostPoints > (rewardsAsPoints + points))
            {
                ZeroOutRewards(userId, points, rewardsBalance);

                return;
            }

            // Calculate the Loyalty Member's new point total by subtracting the points they lost from a return 
            _Rewards.UpdatePoints(loyaltyProgram.CalculateRewards((rewardsAsPoints + points) - lostPoints, out Money? rewards));
            Publish(new LoyaltyMemberLostPoints(this, _MerchantId, userId, points));

            // After calculating the Loyalty Member's new point total, if there is still a Rewards Balance, update
            // their new balance
            if (rewards is null)
                return;

            _Rewards.UpdateBalance(rewards);
            Publish(new LoyaltyMemberLostRewards(this, _MerchantId, userId, rewardsBalance));
        }

        #endregion
    }
}