using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Entitiessss;
using Play.Loyalty.Domain.ValueObjects;
using Play.Loyalty.Contracts.Dtos;
using Play.Domain;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Loyalty.Domain.Enums;

namespace Play.Loyalty.Domain.Entitiesddd;

public class RewardProgram : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly AmountOff _AmountOff;
    private readonly PercentageOff _PercentageOff;
    private bool _IsActive;
    private RewardType _RewardType;
    private PointsPerDollar _PointsPerDollar;
    private uint _RewardThreshold;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private RewardProgram()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardProgram(RewardProgramDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _RewardType = new RewardType(dto.RewardType);
        _AmountOff = new AmountOff(dto.AmountOff);
        _PercentageOff = new PercentageOff(dto.PercentageOff);
        _PointsPerDollar = new PointsPerDollar(dto.PointsPerDollar);
        _RewardThreshold = dto.RewardThreshold;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardProgram(string id, string rewardType, AmountOff amountOff, PercentageOff percentageOff, uint pointsPerDollar, uint rewardThreshold)
    {
        Id = new SimpleStringId(id);
        _RewardType = new RewardType(rewardType);
        _AmountOff = amountOff;
        _PercentageOff = percentageOff;
        _PointsPerDollar = new PointsPerDollar(pointsPerDollar);
        _RewardThreshold = rewardThreshold;
    }

    #endregion

    #region Instance Members

    public void Activate(bool value)
    {
        _IsActive = value;
    }

    internal AmountOff GetAmountOff()
    {
        return _AmountOff;
    }

    internal void UpdatePercentageOff(byte percentage)
    {
        _PercentageOff.UpdatePercentage(percentage);
    }

    internal void UpdateAmountOff(Money amount)
    {
        _AmountOff.UpdateAmount(amount);
    }

    internal void SetRewardType(RewardTypes rewardType)
    {
        _RewardType = new RewardType(rewardType);
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