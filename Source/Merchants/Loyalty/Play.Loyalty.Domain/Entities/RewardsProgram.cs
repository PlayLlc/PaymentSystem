using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.ValueObjects;
using Play.Loyalty.Contracts.Dtos;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;

namespace Play.Loyalty.Domain.Entitiesddd;

public class RewardsProgram : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly RewardAmount _RewardAmount;

    private bool _IsActive;
    private uint _PointsRequired;
    private PointsPerDollar _PointsPerDollar;
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
        _RewardAmount = new RewardAmount(dto.AmountOff);
        _PointsPerDollar = new PointsPerDollar(dto.PointsPerDollar);
        _PointsRequired = dto.RewardThreshold;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardsProgram(string id, RewardAmount rewardAmount, uint pointsPerDollar, uint pointsRequired)
    {
        Id = new SimpleStringId(id);
        _RewardAmount = rewardAmount;
        _PointsPerDollar = new PointsPerDollar(pointsPerDollar);
        _PointsRequired = pointsRequired;
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public void Update(UpdateRewardsProgram command)
    {
        _IsActive = command.IsActive;
        _PointsPerDollar = new PointsPerDollar(command.PointsPerDollar);
        _PointsRequired = command.PointsRequired;
        _RewardAmount.UpdateAmount(command.Reward);
    }

    public void Activate(bool value)
    {
        _IsActive = value;
    }

    internal RewardAmount GetRewardAmount()
    {
        return _RewardAmount;
    }

    internal void UpdatePointsRequired(uint pointsRequired)
    {
        _PointsRequired = pointsRequired;
    }

    internal void UpdatePointsPerDollar(uint pointsPerDollar)
    {
        _PointsPerDollar = new PointsPerDollar(pointsPerDollar);
    }

    internal void UpdateRewardAmount(Money amount)
    {
        _RewardAmount.UpdateAmount(amount);
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override RewardProgramDto AsDto()
    {
        return new RewardProgramDto() {Id = Id};
    }

    #endregion
}