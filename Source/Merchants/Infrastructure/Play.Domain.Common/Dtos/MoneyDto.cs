using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Globalization.Currency;

namespace Play.Inventory.Contracts.Dtos;

public record MoneyDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public ushort NumericCurrencyCode { get; set; }

    [Required]
    public ulong Amount { get; set; }

    #endregion

    #region Constructor

    public MoneyDto()
    { }

    public MoneyDto(Money value)
    {
        NumericCurrencyCode = value.GetNumericCurrencyCode();
        Amount = value.GetAmount();
    }

    #endregion

    #region Instance Members

    public Money AsMoney() => new(Amount, new NumericCurrencyCode(NumericCurrencyCode));

    #endregion

    #region Operator Overrides

    public static implicit operator Money(MoneyDto dto) => new(dto.Amount, new NumericCurrencyCode(dto.NumericCurrencyCode));

    #endregion
}