using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record RemoveVariation
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string VariationId { get; set; } = string.Empty;

    #endregion
}

public record RemoveStockItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string VariationId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    #endregion
}