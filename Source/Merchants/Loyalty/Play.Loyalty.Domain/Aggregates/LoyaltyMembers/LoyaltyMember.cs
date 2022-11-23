using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates._Shared.Rules;
using Play.Loyalty.Domain.Aggregates.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Domain.Aggregates;

public class LoyaltyMember : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly Rewards _Rewards;

    /// <summary>
    ///     The Loyalty Member Number
    /// </summary>
    private readonly RewardsNumber _RewardsNumber;

    private readonly Phone _Phone;
    private Name _Name;
    private Email? _Email;
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
        _RewardsNumber = new RewardsNumber(dto.RewardsNumber);
        _Rewards = new Rewards(dto.Rewards);
        _Name = new Name(dto.Name);
        _Phone = new Phone(dto.Phone);
        _Email = dto.Email is null ? null : new Email(dto.Email);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyMember(string id, string merchantId, string name, string phone, string rewardsNumber, Rewards rewards, string? email = null)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _RewardsNumber = new RewardsNumber(rewardsNumber);
        _Rewards = rewards;
        _Name = new Name(name);
        _Phone = new Phone(phone);
        _Email = email is null ? null : new Email(email);
    }

    #endregion

    #region Instance Members

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task AddRewards(IRepository<LoyaltyProgram, SimpleStringId> loyaltyProgramRepository, UpdateRewardPoints command)
    {
        // Enforce rules

        LoyaltyProgram loyaltyProgram = await loyaltyProgramRepository.GetByIdAsync(new SimpleStringId(command.MerchantId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        uint points = loyaltyProgram.CalculateEarnedPoints(command.TransactionAmount) + _Rewards.GetPoints();
        _Rewards.UpdatePoints(loyaltyProgram.CalculateRewards(points, out Money? reward));

        if (reward is null)
            return;

        _Rewards.AddToBalance(reward);

        // Publish
    }

    public async Task Update(UpdateLoyaltyMember command)
    {
        // Enforce

        _Name = new Name(command.Name);
        _Email = command.Email is null ? null : new Email(command.Email);

        // Publish
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task RemoveRewards(IRepository<LoyaltyProgram, SimpleStringId> loyaltyProgramRepository, UpdateRewardPoints command)
    {
        // Enforce rules

        LoyaltyProgram loyaltyProgram = await loyaltyProgramRepository.GetByIdAsync(new SimpleStringId(command.MerchantId)).ConfigureAwait(false)
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
        LoyaltyProgram loyaltyProgram = await loyaltyProgramRepository.GetByMerchantIdAsync(_MerchantId).ConfigureAwait(false)
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
            RewardsNumber = _RewardsNumber.Value,
            Rewards = _Rewards.AsDto(),
            Name = _Name,
            Email = _Email?.Value,
            Phone = _Phone
        };

    /// <exception cref="ValueObjectException"></exception>
    public static LoyaltyMember Create(CreateLoyaltyMember command)
    {
        // Enforce Rules
        Rewards rewards = new Rewards(GenerateSimpleStringId(), 0, new Money(0, new NumericCurrencyCode(command.NumericCurrencyCode)));

        LoyaltyMember loyaltyMember = new LoyaltyMember(GenerateSimpleStringId(), command.MerchantId, command.Name, command.Phone, command.RewardsNumber,
            rewards, command.Email);

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