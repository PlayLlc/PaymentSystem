using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Entities;

public class Price : Entity<SimpleStringId>
{
    #region Instance Values

    public ulong Amount;
    public NumericCurrencyCode NumericCurrencyCode;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private Price()
    { }

    public Price(PriceDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        NumericCurrencyCode = new NumericCurrencyCode(dto.NumericCurrencyCode);
        Amount = dto.Amount;
    }

    /// <exception cref="ValueObjectException"></exception>
    public Price(string id, Money price)
    {
        Id = new SimpleStringId(id);
        Amount = price.GetAmount();
        NumericCurrencyCode = price.GetNumericCurrencyCode();
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override PriceDto AsDto()
    {
        return new PriceDto()
        {
            Id = Id,
            Amount = Amount,
            NumericCurrencyCode = NumericCurrencyCode
        };
    }

    public override string ToString()
    {
        return Money.AsLocalFormat(Amount, NumericCurrencyCode);
    }

    #endregion
}