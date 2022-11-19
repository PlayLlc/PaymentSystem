using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Contracts.Commands;

public record CreateItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Money Price { get; set; } = null!;

    public string? Description { get; set; } = string.Empty;

    public string? Sku { get; set; } = string.Empty;

    public IEnumerable<CategoryDto> Categories { get; set; } = Array.Empty<CategoryDto>();

    #endregion
}