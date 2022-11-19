using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;

namespace Play.Inventory.Contracts.Commands;

public record CreateVariation
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Money Price { get; set; } = null!;

    public string? Sku { get; set; } = string.Empty;

    #endregion
}