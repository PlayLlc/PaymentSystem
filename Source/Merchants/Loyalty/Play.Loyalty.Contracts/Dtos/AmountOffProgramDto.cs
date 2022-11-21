using Play.Domain;

using System.ComponentModel.DataAnnotations;

using Play.Inventory.Contracts.Dtos;

namespace Play.Loyalty.Contracts.Dtos;

public record AmountOffProgramDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public PriceDto Amount { get; set; }

    [Required]
    public ushort NumericCurrencyCode { get; set; }

    #endregion
}