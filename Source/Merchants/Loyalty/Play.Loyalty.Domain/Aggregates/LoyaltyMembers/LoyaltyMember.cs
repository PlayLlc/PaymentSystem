using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Aggregates;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates._Shared.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Entitiesddd;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Domain.Aggregates;

public class LoyaltyMember : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;
    private readonly Name _Name;
    private readonly RewardsNumber _RewardsNumber;
    private Rewards _Rewards;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private LoyaltyMember()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyMember(LoyaltyMemberDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _MerchantId = new SimpleStringId(dto.MerchantId);
        _Name = new Name(dto.Name);
        _RewardsNumber = new RewardsNumber(dto.RewardsNumber);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyMember(string id, string merchantId, string name, string rewardsNumber)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _Name = new Name(name);
        _RewardsNumber = new RewardsNumber(rewardsNumber);
    }

    #endregion

    #region Instance Members

    internal uint GetPoints() => _Points;

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task AddRewards(IRepository<LoyaltyProgram, SimpleStringId> loyaltyProgramRepository, UpdateRewardPoints command)
    {
        // Enforce rules

        var loyaltyProgram = await loyaltyProgramRepository.GetByIdAsync(new SimpleStringId(command.MerchantId)).ConfigureAwait(false)
                             ?? throw new NotFoundException(typeof(LoyaltyProgram));

        var points = loyaltyProgram.CalculateEarnedPoints(command.TransactionAmount) + _Rewards.GetPoints();
        _Rewards.UpdatePoints(loyaltyProgram.CalculateRewards(points, out Money? reward));

        if (reward is null)
            return;

        _Rewards.AddToBalance(reward);

        // Publish
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task RemoveRewards(IRepository<LoyaltyProgram, SimpleStringId> loyaltyProgramRepository, UpdateRewardPoints command)
    {
        // Enforce rules

        var loyaltyProgram = await loyaltyProgramRepository.GetByIdAsync(new SimpleStringId(command.MerchantId)).ConfigureAwait(false)
                             ?? throw new NotFoundException(typeof(LoyaltyProgram));

        uint lostPoints = loyaltyProgram.CalculateEarnedPoints(command.TransactionAmount) + _Rewards.GetPoints();

        if (lostPoints > 0)
        {
            ConvertRewardsToPoints(loyaltyProgram, lostPoints);

            return;
        }

        _Rewards.SubtractPoints(lostPoints);

        // Publish
    }

    private void ConvertRewardsToPoints(LoyaltyProgram loyaltyProgram, uint lostPoints)
    {
        uint rewardPoints = loyaltyProgram.CalculateEarnedPoints(_Rewards.GetRewardBalance());

        if (lostPoints > rewardPoints)
        {
            _Rewards.UpdatePoints(0);
            _Rewards.SubtractFromBalancce(_Rewards.GetRewardBalance());

            return;
        }

        _Rewards.UpdatePoints(loyaltyProgram.CalculateRewards(rewardPoints - lostPoints, out Money? rewards));

        if (rewards is null)
            return;

        _Rewards.UpdateBalance(rewards);
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task ClaimReward(ILoyaltyProgramRepository loyaltyProgramRepository, ClaimReward command)
    {
        var loyaltyProgram = await loyaltyProgramRepository.GetByMerchantIdAsync(_MerchantId).ConfigureAwait(false)
                             ?? throw new NotFoundException(typeof(LoyaltyProgram));

        Enforce(new RewardsProgramMustBeActiveToClaimReward(loyaltyProgram));
        Enforce(new RewardMustBeGreaterThanOrEqualToClaimAmount(command.RewardAmount, _Rewards.GetRewardBalance()));

        _Rewards.Claim(command.RewardAmount);

        // Publish
    }

    public override SimpleStringId GetId() => Id;

    public override LoyaltyMemberDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            Name = _Name,
            RewardsNumber = _RewardsNumber.Value
        };

    /// <exception cref="ValueObjectException"></exception>
    public static LoyaltyMember Create(CreateLoyaltyMember command)
    {
        var loyaltyMember = new LoyaltyMember(GenerateSimpleStringId(), command.MerchantId, command.Name, command.RewardsNumber);

        // Enforce Rules
        // Publish Domain Event

        return loyaltyMember;
    }

    public static void Remove()
    {
        // Enforce Rules

        // Publish Domain Event
    }

    #endregion
}