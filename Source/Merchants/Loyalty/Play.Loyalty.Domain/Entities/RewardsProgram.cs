using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Domain.Entities;

public class RewardsProgram : Entity<SimpleStringId>
{
    #region Static Metadata

    internal const uint _DefaultPointsRequired = 10000;
    internal const uint _DefaultPointsPerDollar = 100;
    internal const uint _DefaultRewardAmount = 20;

    #endregion

    #region Instance Values

    private Money _RewardAmount;

    private bool _IsActive;
    private uint _PointsRequired;
    private uint _PointsPerDollar;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private RewardsProgram()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardsProgram(RewardProgramDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _RewardAmount = dto.RewardAmount;
        _PointsPerDollar = new PointsPerDollar(dto.PointsPerDollar);
        _PointsRequired = dto.RewardThreshold;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardsProgram(string id, Money rewardAmount, uint pointsPerDollar, uint pointsRequired)
    {
        Id = new SimpleStringId(id);
        _RewardAmount = rewardAmount;
        _PointsPerDollar = new PointsPerDollar(pointsPerDollar);
        _PointsRequired = pointsRequired;
    }

    #endregion

    #region Instance Members

    internal bool IsActive() => _IsActive;

    /// <exception cref="ValueObjectException"></exception>
    public void Update(UpdateRewardsProgram command)
    {
        _IsActive = command.IsActive;
        _PointsPerDollar = new PointsPerDollar(command.PointsPerDollar);
        _PointsRequired = command.PointsRequired;
        _RewardAmount = command.RewardAmount;
    }

    public uint CalculateEarnedPoints(Money transactionAmount) => (uint) (_PointsPerDollar * transactionAmount.GetMajorCurrencyAmount());

    /// <summary>
    /// </summary>
    /// <param name="points">The points earned by the <see cref="LoyaltyMember" /></param>
    /// <param name="reward">New rewards that the <see cref="LoyaltyMember" /> has earned</param>
    /// <returns></returns>
    public uint CalculateRewards(uint points, out Money? reward)
    {
        reward = null;

        if (_PointsRequired > points)
            return points;

        uint rewardCount = points / _PointsRequired;
        reward = _RewardAmount * rewardCount;

        return points - (rewardCount * _PointsRequired);
    }

    public void Activate(bool value)
    {
        _IsActive = value;
    }

    internal Money GetRewardAmount() => _RewardAmount;

    internal void UpdatePointsRequired(uint pointsRequired)
    {
        _PointsRequired = pointsRequired;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal void UpdatePointsPerDollar(uint pointsPerDollar)
    {
        _PointsPerDollar = new PointsPerDollar(pointsPerDollar);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal void UpdateRewardAmount(Money amount)
    {
        if (!amount.IsCommonCurrency(_RewardAmount))
            throw new ValueObjectException($"The {_RewardAmount} could not be updated because the {nameof(amount)} specified was in a different currency");

        _RewardAmount = amount;
    }

    public override SimpleStringId GetId() => Id;

    public override RewardProgramDto AsDto() => new() {Id = Id};

    #endregion
}