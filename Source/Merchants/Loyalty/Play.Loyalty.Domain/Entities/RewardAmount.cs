using Play.Domain.Common.Entitiesd;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Globalization.Currency;
using Play.Domain.ValueObjects;
using Play.Domain.Exceptions;

namespace Play.Loyalty.Domain.Entities;

public class RewardAmount : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly NumericCurrencyCode _NumericCurrencyCode;

    private ulong _Amount;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private RewardAmount()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardAmount(AmountOffDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _Amount = dto.Amount.GetAmount();
        _NumericCurrencyCode = dto.Amount.GetNumericCurrencyCode();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal RewardAmount(string id, Money amount)
    {
        Id = new SimpleStringId(id);
        _Amount = amount.GetAmount();
        _NumericCurrencyCode = amount.GetNumericCurrencyCode();
    }

    #endregion

    #region Instance Members

    internal NumericCurrencyCode GetNumericCurrencyCode()
    {
        return _NumericCurrencyCode;
    }

    public void UpdateAmount(Money amount)
    {
        _Amount = amount.GetAmount();
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override AmountOffDto AsDto()
    {
        return new AmountOffDto()
        {
            Id = Id,
            Amount = new Money(_Amount, _NumericCurrencyCode)
        };
    }

    #endregion
}