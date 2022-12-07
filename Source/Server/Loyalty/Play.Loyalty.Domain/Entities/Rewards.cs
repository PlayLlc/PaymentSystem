using Play.Domain.Common.Dtos;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entities;

public class Rewards : Entity<SimpleStringId>
{
    #region Instance Values

    private uint _Points;

    private MoneyValueObject _Balance;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // private constructor for EF only
    private Rewards()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Rewards(string id, uint points, Money balance)
    {
        Id = new(id);
        _Points = points;
        _Balance = balance;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Rewards(RewardsDto dto)
    {
        Id = new(dto.Id);
        _Balance = dto.Balance.AsMoney();
        _Points = dto.Points;
    }

    #endregion

    #region Instance Members

    internal void UpdatePoints(uint points) => _Points = points;

    internal void SubtractPoints(uint points) => _Points -= points;

    internal void UpdateBalance(Money amount) => _Balance = amount;

    /// <exception cref="ValueObjectException"></exception>
    internal void AddToBalance(Money amount)
    {
        _Balance += amount;
    }

    internal void SubtractFromBalance(Money amount) => _Balance -= amount;

    /// <exception cref="ValueObjectException"></exception>
    internal void Claim(Money amount)
    {
        if (amount > _Balance)
            throw new ValueObjectException($"The {nameof(Rewards)} has an insufficient balance for the amount attempting to be claimed");

        _Balance -= amount;
    }

    internal uint GetPoints() => _Points;
    internal Money GetRewardBalance() => _Balance;

    public override SimpleStringId GetId() => Id;

    public override RewardsDto AsDto() =>
        new()
        {
            Id = Id,
            Balance = new(_Balance),
            Points = _Points
        };

    #endregion
}