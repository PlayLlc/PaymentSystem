using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Inventory.Contracts.Dtos;

public record VariationDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    public PriceDto Price { get; set; } = null!;

    public int Quantity { get; set; }

    #endregion
}