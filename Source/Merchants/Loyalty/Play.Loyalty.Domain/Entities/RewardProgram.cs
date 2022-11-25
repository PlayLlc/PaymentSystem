using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Domain.Entities;

public class RewardProgram : Entity<SimpleStringId>
{
    #region Static Metadata

    internal const uint DefaultPointsRequired = 10000;
    internal const uint DefaultPointsPerDollar = 100;
    internal const uint DefaultRewardAmount = 20;

    #endregion

    #region Instance Values

    private MoneyValueObject _RewardAmount;

    private bool _IsActive;
    private uint _PointsRequired;
    private uint _PointsPerDollar;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private RewardProgram()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardProgram(RewardsProgramDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _RewardAmount = dto.RewardAmount.AsMoney();
        _PointsPerDollar = dto.PointsPerDollar;
        _PointsRequired = dto.RewardThreshold;
        _IsActive = dto.IsActive;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardProgram(string id, Money rewardAmount, uint pointsPerDollar, uint pointsRequired, bool isActive)
    {
        Id = new SimpleStringId(id);
        _RewardAmount = rewardAmount;
        _PointsPerDollar = pointsPerDollar;
        _PointsRequired = pointsRequired;
        _IsActive = false;
    }

    #endregion

    #region Instance Members

    internal bool IsActive() => _IsActive;

    /// <exception cref="ValueObjectException"></exception>
    public void Update(UpdateRewardsProgram command)
    {
        _PointsPerDollar = command.PointsPerDollar;
        _PointsRequired = command.PointsRequired;
        _RewardAmount = command.RewardAmount;
    }

    public uint CalculateEarnedPoints(Money transactionAmount) => (uint) (_PointsPerDollar * transactionAmount.GetMajorCurrencyAmount());

    /// <summary>
    /// </summary>
    /// <param name="points">The points earned by the <see cref="Member" /></param>
    /// <param name="reward">New rewards that the <see cref="Member" /> has earned</param>
    /// <returns></returns>
    public uint CalculateRewards(uint points, out Money? reward)
    {
        reward = null;

        if (_PointsRequired > points)
            return points;

        uint rewardCount = points / _PointsRequired;
        reward = (Money) _RewardAmount * rewardCount;

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
        _PointsPerDollar = pointsPerDollar;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal void UpdateRewardAmount(Money amount)
    {
        if (!amount.IsCommonCurrency(_RewardAmount))
            throw new ValueObjectException($"The {_RewardAmount} could not be updated because the {nameof(amount)} specified was in a different currency");

        _RewardAmount = amount;
    }

    public override SimpleStringId GetId() => Id;

    public override RewardsProgramDto AsDto() => new() {Id = Id};

    #endregion
}