using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Inventory.Contracts.Dtos;

public record PriceDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public ushort NumericCurrencyCode { get; set; }

    [Required]
    public ulong Amount { get; set; }

    #endregion
}