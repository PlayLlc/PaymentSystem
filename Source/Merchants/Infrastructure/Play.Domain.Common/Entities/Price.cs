using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;

namespace Play.Domain.Common.Entities;

public class Price : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly NumericCurrencyCode _NumericCurrencyCode;

    private ulong _Amount;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private Price()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Price(PriceDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _NumericCurrencyCode = new NumericCurrencyCode(dto.Amount.NumericCurrencyCode);
        _Amount = dto.Amount.Amount;
    }

    /// <exception cref="ValueObjectException"></exception>
    public Price(string id, Money price)
    {
        Id = new SimpleStringId(id);
        _Amount = price.GetAmount();
        _NumericCurrencyCode = price.GetNumericCurrencyCode();
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public void Update(Money amount)
    {
        if (!amount.IsCommonCurrency(new Money(_Amount, _NumericCurrencyCode)))
            throw new ValueObjectException(
                $"The {nameof(Price)} couldn't be updated because the {nameof(amount)} provided has a {nameof(NumericCurrencyCode)} of: [{amount.GetNumericCurrencyCode()} but the {nameof(Price)} has a {nameof(NumericCurrencyCode)} of: [{_NumericCurrencyCode}];");

        _Amount = amount.GetAmount();
    }

    public override SimpleStringId GetId() => Id;

    public override PriceDto AsDto() =>
        new()
        {
            Id = Id,
            Amount = new MoneyDto(new Money(_Amount, _NumericCurrencyCode))
        };

    public override string ToString() => Money.AsLocalFormat(_Amount, _NumericCurrencyCode);

    #endregion
}